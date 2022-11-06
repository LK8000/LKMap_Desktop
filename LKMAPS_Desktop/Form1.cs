using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GMap.NET.WindowsForms;
using GMap.NET;
using OSGeo.OGR;
using System.Net;
using System.IO;
using OSGeo.GDAL;
using System.Collections.Specialized;
using System.Diagnostics;
using GMap.NET.WindowsForms.Markers;
using GMap.NET.WindowsForms.ToolTips;
using System.Globalization;
using System.Threading;
using System.IO.Compression;





namespace LKMAPS_Desktop
{


    public partial class LKMAPS_Desktop : Form
    {

        public bool _useOSMRoads = true;
        public bool _useOSMRail = true;
        public bool _useOSMRivers = true;
        public bool _useOSMRLakes = true;
        public bool _useOSMRResidential = true;
        public bool _useOSMRCity = true;

        public string _roadsDetail = "Medium";
        public string _riverDetail = "Medium";
        public string _cityDetail =  "Medium";

        public double _lakesSize = 1;
        public double _citySize = 0.4;

        public double _simplify = 100;

        string _offlineOSMFile = "";
        bool _running = false;
        GMapOverlay _mapAreaOverlay = new GMapOverlay("mapAreaOverlay");
        GMapOverlay _mapTempAreaOverlay = new GMapOverlay("mapTempAreaOverlay");
        GMapOverlay _currentMapsOverlay = new GMapOverlay("currentMapsOverlay");
        double _minLat, _maxLat, _minLon, _maxLon;
        double _tempLat1, _tempLat2, _tempLon1, _tempLon2;
        int _mouseStatus = 0;
        int _step = 0;
        int NSTEPS = 3;
        int _nTiles = 1;
        int _nDownloaded = 0;
        int _cellSize = 0;
        String _mapName = "";
        private bool downloadComplete = false;
        long bytes_total = 0;
        String _labelStatusText = "";
        String _userFolder ;
        String _srtmFolder;
        String _osmFolder;
        string _tmpFolder;
        string _vmapFolder;
        String _outFolder;
        bool _showExistingMaps = true;
        
        double _minWaterArea = 451230;
        bool _fakeProcess = false;


        public LKMAPS_Desktop()
        {
            
            InitializeComponent();
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;


            _userFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\LKMAPS\\";
            bool exists = System.IO.Directory.Exists(_userFolder);
            if (!exists)
                System.IO.Directory.CreateDirectory(_userFolder);
            _srtmFolder = _userFolder + "srtm\\";
            exists = System.IO.Directory.Exists(_srtmFolder);
            if (!exists)
                System.IO.Directory.CreateDirectory(_srtmFolder);
            _osmFolder = _userFolder + "osm\\";
            exists = System.IO.Directory.Exists(_osmFolder);
            if (!exists)
                System.IO.Directory.CreateDirectory(_osmFolder);

            _vmapFolder = _userFolder + "vmap\\";
            exists = System.IO.Directory.Exists(_vmapFolder);
            if (!exists)
                System.IO.Directory.CreateDirectory(_vmapFolder);

            _tmpFolder = _userFolder + "tmp\\";
            exists = System.IO.Directory.Exists(_tmpFolder);
            if (!exists)
                System.IO.Directory.CreateDirectory(_tmpFolder);
            
        }

        public void enableControls(bool enable)
        {
            _running = !enable;
            this.Invoke(new Action(() =>
            {
                foreach (Control ctrl in this.Controls)
                {
                    ctrl.Enabled = enable;
                }            
            }
            ));
        }

        public void setStatus(string text)
        {
            labelStatus.Invoke(new Action(() =>
            {
                labelStatus.Text = text;
            }
            ));

        }

        public void enableControls_notread(bool enable )
        {
            foreach (Control ctrl in this.Controls)
            {
                ctrl.Enabled = enable;
            }            
        }


        private void LKMAPS_Desktop_Load(object sender, EventArgs e)
        {






            

            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            string version = fvi.FileVersion;

            richTextBoxHelp.Text = "Usage :\n";
            richTextBoxHelp.Text += "1) Insert a map name and select the LK8000 '_Map' folder where maps will be created\n";
            richTextBoxHelp.Text += "2) Define your area either using the map or entering the bounding coordinates on the ‘Map area’ group\n";
            richTextBoxHelp.Text += "3) Select Terrain resolution. For high resolution insert 90 meters. No reason to go beyond this resolution.\n";
            richTextBoxHelp.Text += "4) Click ‘Create Terrain”\n";
            richTextBoxHelp.Text += "\n";
            richTextBoxHelp.Text += "Map interraction :\n";
            richTextBoxHelp.Text += "Left Mouse : Select area\n";
            richTextBoxHelp.Text += "Right mouse : Pan \n";
            richTextBoxHelp.Text += "Weel : zoom\n";
            richTextBoxHelp.Text += "\n";
            richTextBoxHelp.Text += "LKMAPS version " +  version + " Tonino Tarsi - 2019\n" ;
            //richTextBoxHelp.Text += "\n";
            richTextBoxHelp.Text += "OpenStreetMap data are distributed under Open Database License\n";
            richTextBoxHelp.Text += "Terrain is from public domain SRTM data\n";






            Updater upd = new Updater(this);
            upd.checkUpdate();

            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            splitContainer1.SplitterDistance = 320;

            _showExistingMaps = Properties.Settings.Default.ShowExistingMaps;
            checkBoxShowExistingMaps.Checked = _showExistingMaps;

            setArea(Properties.Settings.Default.minLat, Properties.Settings.Default.minLon,
                    Properties.Settings.Default.maxLat, Properties.Settings.Default.maxLon);

            textBoxMapName.Text = Properties.Settings.Default.MapName;

            _cellSize = Properties.Settings.Default.CellSize;
            if (_cellSize < 90) _cellSize = 90;
            if (_cellSize > 500) _cellSize = 500;

            labelPixelSize.Text = _cellSize.ToString("00");
            trackBarPixelSize.Value = _cellSize  * 100;

            _outFolder = Properties.Settings.Default.OutFolder;
            textBoxOutFolder.Text = _outFolder;

            _useOSMRoads = Properties.Settings.Default.useOSMRoads;
            _useOSMRail = Properties.Settings.Default.useOSMRail;
            _useOSMRivers = Properties.Settings.Default.useOSMRivers;
            _useOSMRLakes = Properties.Settings.Default.useOSMRLakes;
            _useOSMRResidential = Properties.Settings.Default.useOSMRResidential;
            _useOSMRCity = Properties.Settings.Default.useOSMRCity;
            _roadsDetail = Properties.Settings.Default.roadsDetail;
            _riverDetail = Properties.Settings.Default.riverDetail;
            _cityDetail = Properties.Settings.Default.cityDetail;
            _lakesSize = Properties.Settings.Default.lakesSize;
            _citySize = Properties.Settings.Default.citySize;

            _simplify = Properties.Settings.Default.simplify;

            double meanLat = (_minLat + _maxLat) / 2;
            double meanLon = (_minLon + _maxLon) / 2;


            if (!GMapControl.IsDesignerHosted)
            {
                gmap.Overlays.Add(_mapAreaOverlay);
                gmap.Overlays.Add(_mapTempAreaOverlay);
                gmap.Overlays.Add(_currentMapsOverlay);

                _currentMapsOverlay.IsVisibile = _showExistingMaps;

                gmap.MapProvider = GMap.NET.MapProviders.BingMapProvider.Instance;
                gmap.OnMapZoomChanged += new MapZoomChanged(gmap_OnMapZoomChanged);
                //GMap.NET.GMaps.Instance.Mode = GMap.NET.AccessMode.ServerOnly;
            }
            gmap.Position = new PointLatLng(meanLat, meanLon);

            // get zoom  
            trackBar1.Minimum = gmap.MinZoom * 100;
            trackBar1.Maximum = gmap.MaxZoom * 100;
            trackBar1.TickFrequency = 100;
            trackBar1.Value = 600;

            trackBarPixelSize.Minimum = 90 * 100;
            trackBarPixelSize.Maximum = 500 * 100;
            trackBarPixelSize.TickFrequency = 1000;
            

            gmap.Zoom = 6;

            Splash sp = new Splash();
            sp.TopMost = true;
            sp.StartPosition = FormStartPosition.CenterScreen;;
            sp.Show();

            // Add a link to the LinkLabel.
            LinkLabel.Link link = new LinkLabel.Link();
            link.LinkData = "http://www.lk8000.it";
            linkLabel1.Links.Add(link);


            var gdal_data = System.Environment.GetEnvironmentVariable("GDAL_DATA", EnvironmentVariableTarget.User);
            if (gdal_data == null)
            {

                string appPath = Path.GetDirectoryName(Application.ExecutablePath);
                Environment.SetEnvironmentVariable("GDAL_DATA", appPath + "\\gdal-data", EnvironmentVariableTarget.User);

                System.Diagnostics.Process.Start(Application.ExecutablePath); // to start new instance of application
                Application.Exit();

            }


        }

        void gmap_OnMapZoomChanged()
        {
            trackBar1.Value = (int)(gmap.Zoom * 100.0);
        }

        private void updateMapOverlay()
        {

            List<PointLatLng> mapAreaPoints = new List<PointLatLng>(); ;
            mapAreaPoints.Add(new PointLatLng(_minLat, _minLon));
            mapAreaPoints.Add(new PointLatLng(_maxLat, _minLon));
            mapAreaPoints.Add(new PointLatLng(_maxLat, _maxLon));
            mapAreaPoints.Add(new PointLatLng(_minLat, _maxLon));
            mapAreaPoints.Add(new PointLatLng(_minLat, _minLon));

            GMapPolygon mapArea = new GMapPolygon(mapAreaPoints, "mapArea"); ;
            _mapAreaOverlay.Polygons.Clear();
            _mapAreaOverlay.Polygons.Add(mapArea);


        }

        private void setArea(double lat1, double lon1, double lat2, double lon2)
        {
            _mapAreaOverlay.Clear();

            _minLat = Math.Min(lat1, lat2);
            _maxLat = Math.Max(lat1, lat2);
            _minLon = Math.Min(lon1, lon2);
            _maxLon = Math.Max(lon1, lon2);

            textBoxLonMin.Text = _minLon.ToString("0.00");
            textBoxLonMax.Text = _maxLon.ToString("0.00");
            textBoxLatMin.Text = _minLat.ToString("0.00");
            textBoxLatMax.Text = _maxLat.ToString("0.00");

            updateMapOverlay();

        }

        private void splitContainer1_Panel1_Resize(object sender, EventArgs e)
        {
            gmap.Width = splitContainer1.Panel2.Width;
            gmap.Height = splitContainer1.Panel2.Height;
        }

        private void LKMAPS_Desktop_Resize(object sender, EventArgs e)
        {
            if (splitContainer1.Panel2.Height < 900)
                panel2.Visible = false;
            else
                panel2.Visible = true;

            splitContainer1.SplitterDistance = 320;
            gmap.Width = splitContainer1.Panel2.Width;
            gmap.Height = splitContainer1.Panel2.Height;
        }

        private void gmap_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != System.Windows.Forms.MouseButtons.Left)
                return;

            if ( _running )
                return;

            PointLatLng p = gmap.FromLocalToLatLng(e.X, e.Y);
            switch (_mouseStatus)
            {
                case 0:
                    _mouseStatus = 1;
                    _tempLat1 = p.Lat;
                    _tempLon1 = p.Lng;
                    break;
                case 1:
                    _mouseStatus = 0;
                    _tempLat2 = p.Lat;
                    _tempLon2 = p.Lng;

                    DialogResult dialogResult = MessageBox.Show("Use this area ?", "Map Area", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        UpdateMapArea();
                    }
                    else if (dialogResult == DialogResult.No)
                    {
                        _mapTempAreaOverlay.Clear();
                    }
                    break;

            }   
        }

        private void UpdateMapArea()
        {
            _mapTempAreaOverlay.Clear();
            setArea(_tempLat1, _tempLon1, _tempLat2, _tempLon2);

        }

        private void gmap_MouseUp(object sender, MouseEventArgs e)
        {
            PointLatLng p = gmap.FromLocalToLatLng(e.X, e.Y);
            switch (_mouseStatus)
            {
                case 0:

                    break;
                case 1:
                    break;

            }   

        }

        private void gmap_MouseMove(object sender, MouseEventArgs e)
        {
            PointLatLng p = gmap.FromLocalToLatLng(e.X, e.Y);
            switch (_mouseStatus)
            {
                case 0:
                    break;
                case 1:
                    _tempLat2 = p.Lat;
                    _tempLon2 = p.Lng;

                    List<PointLatLng> mapAreaPoints = new List<PointLatLng>();
                    mapAreaPoints.Add(new PointLatLng(Math.Min(_tempLat1, _tempLat2), Math.Min(_tempLon1, _tempLon2)));
                    mapAreaPoints.Add(new PointLatLng(Math.Max(_tempLat1, _tempLat2), Math.Min(_tempLon1, _tempLon2)));
                    mapAreaPoints.Add(new PointLatLng(Math.Max(_tempLat1, _tempLat2), Math.Max(_tempLon1, _tempLon2)));
                    mapAreaPoints.Add(new PointLatLng(Math.Min(_tempLat1, _tempLat2), Math.Max(_tempLon1, _tempLon2)));
                    mapAreaPoints.Add(new PointLatLng(Math.Min(_tempLat1, _tempLat2), Math.Min(_tempLon1, _tempLon2)));

                    GMapPolygon mapArea = new GMapPolygon(mapAreaPoints, "Map Area");

                    _mapTempAreaOverlay.Clear();
                    _mapTempAreaOverlay.Polygons.Add(mapArea);

                    break;

            }   
        }

        private void textBoxLatMax_TextChanged(object sender, EventArgs e)
        {
            if (!textBoxLatMax.Focused)
                return;
            try
            {
                _maxLat = Convert.ToDouble(textBoxLatMax.Text);
                updateMapOverlay();
            }
            catch
            {
            }
        }

        private void textBoxLonMax_TextChanged(object sender, EventArgs e)
        {
            if (!textBoxLonMax.Focused)
                return;
            try
            {
                _maxLon = Convert.ToDouble(textBoxLonMax.Text);
                updateMapOverlay();
            }
            catch
            {

            }
        }

        private void textBoxLatMin_TextChanged(object sender, EventArgs e)
        {
            if (!textBoxLatMin.Focused)
                return;
            try
            {
                _minLat = Convert.ToDouble(textBoxLatMin.Text);
                updateMapOverlay();
            }
            catch
            {

            }
        }

        private void textBoxLonMin_TextChanged(object sender, EventArgs e)
        {
            if (!textBoxLonMin.Focused)
                return;
            try
            {
                _minLon = Convert.ToDouble(textBoxLonMin.Text);
                updateMapOverlay();
            }
            catch
            {

            }
        }

        private void buttonSelectOutFolder_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !String.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    textBoxOutFolder.Text =  fbd.SelectedPath;
                    updateCurrentMapsOverlay();
                }
            }
        }

        private void LKMAPS_Desktop_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.minLat = _minLat;
            Properties.Settings.Default.minLon = _minLon;
            Properties.Settings.Default.maxLat = _maxLat;
            Properties.Settings.Default.maxLon = _maxLon;
            Properties.Settings.Default.CellSize = _cellSize;
            Properties.Settings.Default.MapName = textBoxMapName.Text;
            Properties.Settings.Default.OutFolder = textBoxOutFolder.Text;
            Properties.Settings.Default.ShowExistingMaps = _showExistingMaps;


            Properties.Settings.Default.useOSMRoads 	=	_useOSMRoads  	;
            Properties.Settings.Default.useOSMRail 	=	_useOSMRail  	;
            Properties.Settings.Default.useOSMRivers 	=	_useOSMRivers  	;
            Properties.Settings.Default.useOSMRLakes 	=	_useOSMRLakes  	;
            Properties.Settings.Default.useOSMRResidential 	=	_useOSMRResidential  	;
            Properties.Settings.Default.useOSMRCity 	=	_useOSMRCity  	;
            Properties.Settings.Default.roadsDetail 	=	_roadsDetail  	;
            Properties.Settings.Default.riverDetail 	=	_riverDetail  	;
            Properties.Settings.Default.cityDetail 	=	_cityDetail  	;
            Properties.Settings.Default.lakesSize 	=	_lakesSize  	;
            Properties.Settings.Default.citySize	=	_citySize 	;

            Properties.Settings.Default.simplify = _simplify;

            Properties.Settings.Default.Save();

            
        }

        private void buttonCreateTerrain_Click(object sender, EventArgs e)
        {
            enableControls(false);
            _cellSize = Convert.ToInt32(labelPixelSize.Text);
            _outFolder = textBoxOutFolder.Text;
            _nDownloaded = 0;

            if (!Directory.Exists(_outFolder))
            {
                Directory.CreateDirectory(_outFolder);
            }


            _mapName = textBoxMapName.Text;

            backgroundWorker1.RunWorkerCompleted += worker_RunWorkerCompleted; 


            backgroundWorker1.RunWorkerAsync();


        }



        static void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

            //enableControls(false);
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            

            _step = 0; // Download SRTM

            List<String> tiles = new List<string>();
            List<String> download_tiles;

            if (_cellSize < 90)
            {
                download_tiles = GDALUtil.getSRTM30Tiles(_minLat, _minLon, _maxLat, _maxLon);
            }
            else
            {
                download_tiles = GDALUtil.getSRTM90Tiles(_minLat, _minLon, _maxLat, _maxLon);

            }

            _nTiles = download_tiles.Count();

            foreach (String t in download_tiles)
            {
                setStatus("Downloading " + t); 
                backgroundWorker1.ReportProgress(5);
                downloadComplete = false;
                if (_cellSize < 90)
                {
                    DownloadSRTM30(t);
                }
                else
                {
                    List<String> tmpList = DownloadSRTM90(t);
                    foreach (String steTmp in tmpList)
                    {
                        tiles.Add(steTmp);
                    }
                    
                }





                SetProgress(100);
                _nDownloaded++;
            }
            backgroundWorker1.ReportProgress(0);


            _step = 1;  // Mosaic 
            setStatus("Mosaicing ... please wait");
            MosaicDTM(_srtmFolder, tiles, _mapName, _minLat, _minLon, _maxLat, _maxLon, _cellSize);

            _step = 2;  // map 
            setStatus("Generating map ... please wait");
            Export2DEM(_srtmFolder + _mapName + ".tif", _outFolder , _mapName + ".DEM");


            _step = NSTEPS;
            SetProgress(100);

            updateCurrentMapsOverlay();


            enableControls(true);

            MessageBox.Show("Done");

           
        }

        private void Export2DEM(string tiff_file, String outFolder, string mapName )
        {
            BinaryWriter writer = new BinaryWriter(File.Open(outFolder + "/" + mapName , FileMode.Create));
            
            Gdal.AllRegister();
            Dataset ds = Gdal.Open(tiff_file, Access.GA_ReadOnly);
            Band ba = ds.GetRasterBand(1);
            double[] geotransform = new double[6];
            ds.GetGeoTransform(geotransform);


            double Left = 0;
            double Right = 0;
            double Top = 0;
            double Bottom = 0;
            double StepSize = 0;
            uint Rows = 0;
            uint Columns = 0;

            Columns = (uint)ds.RasterXSize;
            Rows = (uint)ds.RasterYSize;

            
            Left  = geotransform[0];
            Top   = geotransform[3];
            Right = geotransform[0] + Columns * geotransform[1];
            Bottom = geotransform[3] + Rows * geotransform[5];
            StepSize = geotransform[1];

            writer.Write(Left);
            writer.Write(Right);
            writer.Write(Top);
            writer.Write(Bottom);
            writer.Write(StepSize);
            writer.Write(Rows);
            writer.Write(Columns);



            short[] data = new short[Columns];

            for (int j = 0; j < Rows; j++)
            {
                SetProgress((int)(j * 100.0 / Rows));

                ba.ReadRaster(0, j, (int)Columns, 1, data, (int)Columns, 1, 0, 0);

                for (int k = 0; k < Columns; k++)
                {
                    ushort pix = (ushort)data[k];
                    writer.Write(pix);
                }
            }


 /*           short[] data = new short[Rows];
            for (int c = 0; c < Columns; c++)
            {
                SetProgress((int)(c * 100.0 / Columns));

                ba.ReadRaster(c, 0, 1, (int)Rows, data, 1, (int)Rows, 0, 0);
                for (int i = 0; i < Rows; i++)
                {
                    ushort pix = (ushort)data[i];
                    writer.Write(pix);
                }
            }
   */           

/*            short[] data = new short[Columns * Rows];
            ba.ReadRaster(0, 0, (int)Columns, (int)Rows, data, (int)Columns, (int)Rows, 0, 0);

            for (int i = 0; i < Columns; i++)
	        {
                SetProgress( (int) ( i * 100.0 / Columns));
                for (int j = 0; j < Rows; j++)
                {
                    ushort pix = (ushort)data[i * Rows + j];
                    writer.Write(pix);
                }
	        }
*/


            writer.Close();
            ba.FlushCache();
	        ds.FlushCache();
            ds.Dispose();
            ds = null;
            ba = null;
        }

        private void MosaicDTM(string srtmFolder, List<string> tiles, string mapName, double lry, double ulx, double uly, double lrx, int cellSize)
        {
            Gdal.AllRegister();
            OSGeo.GDAL.Driver drv = Gdal.GetDriverByName("GTiff");

            double cx = cellSize * 0.0008333333 / 90;

            int w = (int)((lrx - ulx) / cx + 0.5);
            int h = (int)((uly - lry) / cx + 0.5);

            if (File.Exists(srtmFolder + mapName + ".tif"))
            {
                File.Delete(srtmFolder + mapName + ".tif");
            }

            Dataset t_ds = drv.Create(srtmFolder+mapName+".tif", w, h, 1, DataType.GDT_UInt16,null);
            Band t_ba = t_ds.GetRasterBand(1);

          
            double[] t_geotransform = new double[6];
            t_geotransform[0] = ulx ;/* top left x */
            t_geotransform[1] = cx ;/* w-e pixel resolution */
            t_geotransform[2] = 0;/* 0 */
            t_geotransform[3] = uly;/* top left y */
            t_geotransform[4] = 0;/* 0 */
            t_geotransform[5] =  -cx;/* n-s pixel resolution (negative value) */
            double t_fh_RasterXSize  = cx;
            double t_fh_RasterYSize = cx;
            double t_ulx = t_geotransform[0];
            double t_uly = t_geotransform[3];
            double t_lrx = t_geotransform[0] + w * t_geotransform[1];
            double t_lry = t_geotransform[3] + h * t_geotransform[5];
            t_ds.SetGeoTransform(t_geotransform);

            int ct = 0;
            foreach (String t in tiles)
            {
                SetProgress((int)(ct * 100.0 / tiles.Count));

                String fileName = "/vsizip/" + _srtmFolder + t  ;
                if (_cellSize < 90)
                {
                    String tt = t.Split('/')[1];
                    fileName = "/vsizip/" + _srtmFolder + tt + "\\" + Path.GetFileNameWithoutExtension(tt);
                }
                
                Dataset  s_ds= Gdal.Open(fileName, Access.GA_ReadOnly);
                Band s_ba = s_ds.GetRasterBand(1);
                double[] s_geotransform = new double[6];
                s_ds.GetGeoTransform(s_geotransform);
                double s_ulx = s_geotransform[0];
                double s_uly = s_geotransform[3];
                double s_lrx = s_ulx + s_geotransform[1] * s_ds.RasterXSize;
                double s_lry = s_uly + s_geotransform[5] * s_ds.RasterYSize;

                // figure out intersection region
                double  tgw_ulx = Math.Max(t_ulx,s_ulx);
                double tgw_lrx = Math.Min(t_lrx, s_lrx);
                double  tgw_uly;
                double  tgw_lry;
                if ( t_geotransform[5] < 0 ) 
                {
                    tgw_uly = Math.Min(t_uly,s_uly);
                    tgw_lry = Math.Max(t_lry,s_lry);
                }
                else 
                {
                    tgw_uly = Math.Max(t_uly,s_uly);
                    tgw_lry = Math.Min(t_lry,s_lry);
                }

                // do they even intersect?
                if ( tgw_ulx >= tgw_lrx ) 
                    continue;
                if ( t_geotransform[5] < 0 && tgw_uly <= tgw_lry ) 
                    continue;
                if ( t_geotransform[5] > 0 && tgw_uly >= tgw_lry ) 
                     continue;

                // compute target window in pixel coordinates.
                int tw_xoff = (int)((tgw_ulx - t_geotransform[0]) / t_geotransform[1] + 0.1);
                int tw_yoff = (int)((tgw_uly - t_geotransform[3]) / t_geotransform[5] + 0.1);
                int tw_xsize = (int)((tgw_lrx - t_geotransform[0]) / t_geotransform[1] + 0.5) - tw_xoff;
                int tw_ysize = (int)((tgw_lry - t_geotransform[3]) / t_geotransform[5] + 0.5) - tw_yoff;

                if ( tw_xsize < 1 || tw_ysize < 1 )
                    continue;

                // Compute source window in pixel coordinates.
                int sw_xoff = (int)((tgw_ulx - s_geotransform[0]) / s_geotransform[1]);
                int sw_yoff = (int)((tgw_uly - s_geotransform[3]) / s_geotransform[5]);
                int sw_xsize = (int)((tgw_lrx - s_geotransform[0]) / s_geotransform[1] + 0.5) - sw_xoff;
                int sw_ysize = (int)((tgw_lry - s_geotransform[3]) / s_geotransform[5] + 0.5) - sw_yoff;

                if ( sw_xsize < 1 ||  sw_ysize < 1 )
                    continue;

                short[] data = new short[sw_xsize * sw_ysize];

                s_ba.ReadRaster(sw_xoff, sw_yoff, sw_xsize, sw_ysize,data, sw_xsize, sw_ysize, 0, 0 );

                t_ba.WriteRaster(tw_xoff, tw_yoff, tw_xsize, tw_ysize, data, sw_xsize, sw_ysize, 0, 0);

                s_ba.FlushCache();
                s_ds.FlushCache();
                s_ba.Dispose();
                s_ds.Dispose();
                s_ds = null;
                s_ba = null;
            }

 
            t_ba.FlushCache();
            t_ds.FlushCache();
            t_ba.Dispose();
            t_ds.Dispose();
            t_ba = null;
            t_ds = null;
        }



        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            buttonCreateTerrain.Text = "Create Terrain";
   
        }




        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBarPartial.Value = e.ProgressPercentage;
            progressBarTotal.Value = 100 * _step / NSTEPS;
            labelStatus.Text = _labelStatusText;
        }

        public List<String> DownloadSRTM90(String theTile)
        {
            List<String> retListOfTiles = new List<string>();
            String tile = theTile;
            //String remoteUri = "ftp://srtm.csi.cgiar.org/SRTM_v41/SRTM_Data_GeoTIFF/";
            //String remoteUri = "http://srtm.csi.cgiar.org/SRT-ZIP/SRTM_V41/SRTM_Data_GeoTiff/";
            //String remoteUri = "http://srtm.csi.cgiar.org/wp-content/uploads/files/srtm_5x5/TIFF/";
            //String remoteUri = "http://data.gis-lab.ru/srtm-tif/";
            String remoteUri = "http://viewfinderpanoramas.org/";

            if (tile.Contains('/') ) {
                remoteUri += tile.Split('/')[0] + "/"; 
                tile = tile.Split('/')[1];
            }

            String zipName = tile;
            if (!tile.Contains(".zip"))
            {
                zipName = tile + ".zip";
            }

            if ( ! File.Exists(_srtmFolder + zipName)) {
                DownloadFile(remoteUri, zipName, _srtmFolder);
            }

            using (ZipArchive archive = ZipFile.OpenRead(_srtmFolder + zipName))
            {
                foreach (ZipArchiveEntry entry in archive.Entries)
                {
                    String fullname = entry.FullName;
                    if (fullname.EndsWith(".tif") || fullname.EndsWith(".hgt"))
                    {
                        retListOfTiles.Add(zipName + "\\" + fullname);
                    }
                }
            }

            return retListOfTiles;

        }

        public void DownloadSRTM30(String tile)
        {
            // http://dds.cr.usgs.gov/srtm/version2_1/SRTM3/Eurasia/ 
            //String remoteUri = "http://dds.cr.usgs.gov/srtm/version2_1/SRTM3/";
            String remoteUri = "ftp://ftp.uni-duisburg.de/GIS/GISData/SRTM/version2_1/HGT/SRTM3/";
            string[] words = tile.Split('/');
            if (words.Length != 2)
                return;
            remoteUri +=  words[0] + "/";
            String fileName = words[1];
            DownloadFile(remoteUri, fileName, _srtmFolder);
        }

        private void startFakeProgress()
        {
            _fakeProcess = true;
            
            backgroundWorkerFakeProgress.RunWorkerAsync();
        }
        private void stopsFakeProgress()
        {
            _fakeProcess = false;

            if (backgroundWorkerFakeProgress.IsBusy == true)
            {

                backgroundWorkerFakeProgress.CancelAsync();
                while (backgroundWorkerFakeProgress.IsBusy == true)
                {
                    backgroundWorkerFakeProgress.CancelAsync();
                }

                backgroundWorkerFakeProgress.Dispose();

            }
            
        }
        private void backgroundWorkerFakeProgress_DoWork(object sender, DoWorkEventArgs e)
        {
            int i = 0;
            while (_fakeProcess)
            {
                i++;
                if (i > 100) i = 0;
                if (_fakeProcess) SetProgress(Math.Min(i,100));
                i += 10;
                Thread.Sleep(1000);
                
            }
        }

        public void DownloadFile(String remoteUri, String theFile, String location)
        {
            setStatus("Dowloading " + theFile + "...");

            bool bFTP = false;
            if ( remoteUri.Substring(0,3) == "ftp" )
                bFTP = true;

            downloadComplete = false;
            String myStringWebResource = null;
            //WebClient myWebClient = new WebClient();
            //myWebClient.Credentials = new NetworkCredential("toninotarsi1", "Giulia.2004");
            myStringWebResource = remoteUri + theFile;
            Uri URL = new Uri(myStringWebResource);

            if ( bFTP )
            {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(URL);
                request.Proxy = null;
                request.Method = WebRequestMethods.Ftp.GetFileSize;
                try
                {
                    FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                    Stream responseStream = response.GetResponseStream();
                    bytes_total = response.ContentLength; //this is an int member variable stored for later
                    Console.WriteLine("Fetch Complete, ContentLength {0}", response.ContentLength);
                    response.Close();
                }
                catch (Exception ex)
                {
                    bytes_total = 2000000;
                }


            }

            using (WebClient webClient = new WebClient())
            {

                //byte[] response = webClient.UploadValues("https://urs.earthdata.nasa.gov/oauth/authorize?", values);
                //string r = System.Text.Encoding.UTF8.GetString(response);


                webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(Completed);
                webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);

                



                if ( checkZipDataSet(location + theFile, theFile.Split('.')[0]))
                {
                    backgroundWorker1.ReportProgress(90);
                    downloadComplete = true;
                    return;
                }

               
                try
                {
                    // Start downloading the file
                    webClient.DownloadFileAsync(URL, location +  theFile);
                    while (!downloadComplete)
                    {
                        Application.DoEvents();
                    }
                    downloadComplete = false;

                }

   
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private bool checkZipDataSet(string zipnale,string tile)
        {
            try
            {
                String theDS = "/vsizip/" + zipnale + "\\" + tile + ".tif";
                if (_cellSize < 90)
                {
                    theDS = "/vsizip/" + zipnale + "\\" + tile + ".hgt"; ;
                }
                Gdal.AllRegister();
                Dataset ds = Gdal.Open(theDS, Access.GA_ReadOnly);
                Band ba = ds.GetRasterBand(1);
                double[] geotransform = new double[6];
                ds.GetGeoTransform(geotransform);
                ds.Dispose();

            }
            catch
            {
                return false;
            }


            return true;
        }

        private void ProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            int prog;
            if ( e.TotalBytesToReceive == -1 ) 
                prog = (int)(((float)e.BytesReceived / (float)bytes_total) * 100.0);
            else
                prog = (int)(((float)e.BytesReceived / (float)e.TotalBytesToReceive) * 100.0);
            SetProgress(Math.Max(3,prog));
 //           if ( e.BytesReceived == bytes_total )
 //               downloadComplete = true;
        }




        public void SetProgressDownload(int prog)
        {
            this.progressBarPartial.Value = prog;
        }

        delegate void SetProgressCallback(int p);
        private void SetProgress(int text)
        {

            if (this.progressBarPartial.InvokeRequired)
            {
                SetProgressCallback d = new SetProgressCallback(SetProgress);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                if (text > 100)
                    text = text - 100;

                try
                {
                    this.progressBarPartial.Value = Math.Min(100, text);
                    //this.progressBarTotal.Value = Math.Min(100, text);
                    this.progressBarTotal.Value = Math.Min(100, (int)(100 * ((double)_step / (double)NSTEPS)));
                }
                catch
                {
                }
            }
        }

        private void Completed(object sender, AsyncCompletedEventArgs e)
        {
            //stopsFakeProgress();
            downloadComplete = true;


 /*           if (e.Cancelled == true)
            {
                MessageBox.Show("Download has been canceled.");
            }
            else
            {
                MessageBox.Show("Download completed!");
            }*/
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void buttonZoomUp_Click(object sender, EventArgs e)
        {
            gmap.Zoom = ((int)gmap.Zoom) + 1;
        }

        private void buttonZoomDown_Click(object sender, EventArgs e)
        {
            gmap.Zoom = ((int)(gmap.Zoom + 0.99)) - 1;
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            gmap.Zoom = trackBar1.Value / 100.0;
        }

        private void LKMAPS_Desktop_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(e.Link.LinkData as string);
        }

        private void textBoxOutFolder_TextChanged(object sender, EventArgs e)
        {
            updateCurrentMapsOverlay();
        }



        private void updateCurrentMapsOverlay()
        {
            if (!System.IO.Directory.Exists(_outFolder))
                return;

            _currentMapsOverlay.Clear();

            DirectoryInfo d = new DirectoryInfo(_outFolder);
            FileInfo[] Files = d.GetFiles("*.DEM");

            foreach (FileInfo f in Files)
            {
                string fileName= f.FullName;
                try
                {
                    double Left;
                    double Right;
                    double Top;
                    double Bottom;
                    double StepSize;
                    uint Rows;
                    uint Columns;

                    if (File.Exists(fileName))
                    {
                        using (BinaryReader reader = new BinaryReader(File.Open(fileName, FileMode.Open)))
                        {
                            Left = reader.ReadDouble();
                            Right = reader.ReadDouble();
                            Top = reader.ReadDouble();
                            Bottom = reader.ReadDouble();
                            StepSize = reader.ReadDouble();
                            Rows = reader.ReadUInt32();
                            Columns = reader.ReadUInt32();

                            reader.Close();

                            List<PointLatLng> mapAreaPoints = new List<PointLatLng>(); ;
                            mapAreaPoints.Add(new PointLatLng(Bottom, Left));
                            mapAreaPoints.Add(new PointLatLng(Top, Left));
                            mapAreaPoints.Add(new PointLatLng(Top, Right));
                            mapAreaPoints.Add(new PointLatLng(Bottom, Right));
                            mapAreaPoints.Add(new PointLatLng(Bottom, Left));
                            GMapPolygon pol = new GMapPolygon(mapAreaPoints, f.Name); ;
                            pol.Fill = new SolidBrush(Color.FromArgb(0, Color.Red));
                            pol.Stroke = new Pen(Color.Red, 1);
                            _currentMapsOverlay.Polygons.Add(pol);

                            

                            GMap.NET.WindowsForms.GMapMarker marker = new GMarkerGoogle(new PointLatLng((Top+Bottom)/2, (Left+Right)/2),
                                                                        GMarkerGoogleType.red_small);
                            int cellSize = (int)(StepSize * 90 / 0.0008333333);

                            marker.ToolTipText = f.Name + "\n" + 
                                "CellSize=" + cellSize.ToString(CultureInfo.InvariantCulture) + "\n" +
                                "Left=" + Left.ToString(CultureInfo.InvariantCulture) + "\n" +
                                "Right=" + Right.ToString(CultureInfo.InvariantCulture) + "\n" +
                                "Top=" + Top.ToString(CultureInfo.InvariantCulture) + "\n" +
                                "Bottom=" + Bottom.ToString(CultureInfo.InvariantCulture) + "\n" 
                                ;

                            marker.Tag = cellSize.ToString(CultureInfo.InvariantCulture) + "," + 
                                Left.ToString(CultureInfo.InvariantCulture) + "," + 
                                Right.ToString(CultureInfo.InvariantCulture) + "," +
                                Top.ToString(CultureInfo.InvariantCulture) + ","
                                + Bottom.ToString(CultureInfo.InvariantCulture) + ",";


                            _currentMapsOverlay.Markers.Add(marker);

                        }
                    }

                }
                catch
                {
                }
            }

        }

        private void gmap_OnMarkerClick(GMapMarker item, MouseEventArgs e)
        {

            _mouseStatus = 0;


            _tempLat1 = Convert.ToDouble(item.Tag.ToString().Split(',')[4]);

            _tempLon1 =  Convert.ToDouble(item.Tag.ToString().Split(',')[1]);;

            _tempLat2 =  Convert.ToDouble(item.Tag.ToString().Split(',')[3]);;
            _tempLon2 =  Convert.ToDouble(item.Tag.ToString().Split(',')[2]);;


            UpdateMapArea();

        
        }


        private void checkBoxShowExistingMaps_CheckedChanged(object sender, EventArgs e)
        {
            _currentMapsOverlay.IsVisibile = checkBoxShowExistingMaps.Checked;
            _showExistingMaps = checkBoxShowExistingMaps.Checked;

        }


        private void buttonCreateTopology_Click(object sender, EventArgs e)
        {

            if (!checkAPIStatus())
                return;
            
            double area = (_maxLat - _minLat) * (_maxLon - _minLon);
            if (area > 5)
            {
                DialogResult dialogResult = MessageBox.Show(" Your area is very big and may require long to process\nPlease consider using the offline method\nTimeout on server is set to 1 hour\nIf you do not receive data after that time you can close the program\nContinue?", "Big area", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.No)
                {
                    return;
                }
            }

            //enableControls(false);
            _cellSize = Convert.ToInt32(labelPixelSize.Text);
            _outFolder = textBoxOutFolder.Text;
            _nDownloaded = 0;
            _mapName = textBoxMapName.Text;

            if (!Directory.Exists(_userFolder + "/" + _mapName))
            {
                Directory.CreateDirectory(_userFolder + "/" + _mapName);
            }

            //backgroundWorkerLKM.RunWorkerAsync();

            backgroundWorkerOSM.RunWorkerAsync();


        }


        private void backgroundWorkerOSM_DoWork(object sender, DoWorkEventArgs e)
        {
            enableControls(false);

            _step = 1; // Download OSM
            _nTiles = 1;
            NSTEPS = 8;
            string api_url;
            string bbox = _minLon.ToString(CultureInfo.InvariantCulture) + "," + _minLat.ToString(CultureInfo.InvariantCulture) + "," +
                _maxLon.ToString(CultureInfo.InvariantCulture) + "," + _maxLat.ToString(CultureInfo.InvariantCulture);


            startFakeProgress();

            api_url = "http://www.overpass-api.de/api/interpreter?data=[bbox][maxsize:1073741824][out:xml][timeout:3600];";
            api_url += "(";

            if (_useOSMRoads)
            {
                if (_roadsDetail == "Low")
                    api_url +=  "way[highway=motorway];way[highway=trunk];";
                if (_roadsDetail == "Medium")
                    api_url += "way[highway=motorway];way[highway=trunk];way[highway=primary];";
                if (_roadsDetail == "High")
                    api_url += "way[highway=motorway];way[highway=trunk];way[highway=primary];way[highway=primary];";
                if (_roadsDetail == "VeyHigh")
                    api_url += "way[highway=motorway];way[highway=trunk];way[highway=primary];way[highway=secondary];way[highway=tertiary];";
            }

            if (_useOSMRail )
            {
                api_url += "way[railway=rail];";
            }

            if (_useOSMRivers)
            {
                //if (_riverDetail == "High")
                //    api_url += "way[\"waterway\"~\"^(river)$\"];";
                //else
                api_url += "rel[waterway=river];";
            }

            if (_useOSMRLakes)
            {
                api_url += "rel[natural=water];";
            }

            if (_useOSMRCity)
            {
                if (_cityDetail == "High")
                    api_url += "node[\"place\"~\"^(city|town|village)$\"];";
                else  // (_cityDetail == "Medium")
                    api_url += "node[\"place\"~\"^(city|town)$\"];";
            }

            if (_useOSMRResidential)
            {
                api_url += "way[landuse=residential];";  //  "way[\"landuse\"~\"^(residential)$\"];";
            }

            api_url += ");";
            api_url += "(._;>;);out;&bbox=" + bbox;


            downloadOSM(System.Uri.EscapeUriString(api_url), "osm.osm", _osmFolder);

            _step++;
            processOSM(_osmFolder + "osm.osm", _mapName,  _minLon, _minLat, _maxLon, _maxLat);
            _step++;

            stopsFakeProgress();

            _step++;
            processOcean(_mapName, _minLon, _minLat, _maxLon, _maxLat);

            _step++;
            createTBLFile(_mapName);

            _step++;
            //createFinalZip(_mapName, _outFolder);
            createFinalZipNew(_mapName, _outFolder);

            _step = NSTEPS;


            enableControls(true);

            _step = NSTEPS;
            SetProgress(100);

            MessageBox.Show("Done");

        }

        private void processOSM(string osmFile, string mapName,double minLon, double minLat, double maxLon, double maxLat)
        {
            // Configure OGR
            string encoding = "UTF-8";
            Gdal.SetConfigOption("OGR_INTERLEAVED_READING", "YES");
            Gdal.SetConfigOption("OSM_COMPRESS_NODES", "YES");
            Gdal.SetConfigOption("CPL_TMPDIR", _tmpFolder);
            Gdal.SetConfigOption("OSM_MAX_TMPFILE_SIZE", "0");
            //Gdal.SetConfigOption("SHAPE_ENCODING", "ISO-8859-1");
            OSGeo.GDAL.Gdal.SetConfigOption("GDAL_FILENAME_IS_UTF8", "YES");
            Gdal.SetConfigOption("SHAPE_ENCODING", "UTF-8");
            Gdal.SetConfigOption("ENCODING", "UTF-8");


            Ogr.RegisterAll();

            OSGeo.OGR.Driver drv = Ogr.GetDriverByName("ESRI Shapefile");
            OSGeo.OSR.SpatialReference srs = new OSGeo.OSR.SpatialReference(null);
            srs.ImportFromEPSG(4326);
            OSGeo.OGR.FieldDefn fld_level = new OSGeo.OGR.FieldDefn("LEVEL", FieldType.OFTString);
            OSGeo.OGR.FieldDefn fld_area = new OSGeo.OGR.FieldDefn("AREA", FieldType.OFTReal);
            OSGeo.OGR.Geometry ring = new OSGeo.OGR.Geometry(OSGeo.OGR.wkbGeometryType.wkbLinearRing);
            ring.AddPoint(minLon, minLat, 0);
            ring.AddPoint(minLon, maxLat, 0);
            ring.AddPoint(maxLon, maxLat, 0);
            ring.AddPoint(maxLon, minLat, 0);
            ring.AddPoint(minLon, minLat, 0);
            OSGeo.OGR.Geometry poly = new OSGeo.OGR.Geometry(OSGeo.OGR.wkbGeometryType.wkbPolygon);
            poly.AssignSpatialReference(srs);
            poly.AddGeometry(ring);


            //ROADS
            DataSource roadbig_line = drv.CreateDataSource(_userFolder + mapName + @"\roadbig_line.shp", null);
            Layer lyr_roadbig_line = roadbig_line.CreateLayer("roadbig_line", srs, wkbGeometryType.wkbLineString, null);
            lyr_roadbig_line.CreateField(fld_level, 0);
            createCPG(_userFolder + mapName + @"\roadbig_line.cpg", encoding);

            DataSource roadmedium_line = drv.CreateDataSource(_userFolder + mapName + @"\roadmedium_line.shp", null);
            Layer lyr_roadmedium_line = roadbig_line.CreateLayer("roadmedium_line", srs, wkbGeometryType.wkbLineString, null);
            lyr_roadmedium_line.CreateField(fld_level, 0);
            createCPG(_userFolder + mapName + @"\roadmedium_line.cpg", encoding);

            DataSource roadsmall_line = drv.CreateDataSource(_userFolder + mapName + @"\roadsmall_line.shp", null);
            Layer lyr_roadsmall_line = roadsmall_line.CreateLayer("roadsmall_line", srs, wkbGeometryType.wkbLineString, null);
            lyr_roadsmall_line.CreateField(fld_level, 0);
            createCPG(_userFolder + mapName + @"\roadsmall_line.cpg", encoding);

            // RAILROAS
            DataSource railroad_line = drv.CreateDataSource(_userFolder + mapName + @"\railroad_line.shp", null);
            Layer lyr_railroad_line = railroad_line.CreateLayer("railroad_line", srs, wkbGeometryType.wkbLineString, null);
            lyr_railroad_line.CreateField(fld_level, 0);
            createCPG(_userFolder + mapName + @"\railroad_line.cpg", encoding);

            // RIVER
            DataSource water_line = drv.CreateDataSource(_userFolder + mapName + @"\water_line.shp", null);
            Layer lyr_water_line = water_line.CreateLayer("water_line", srs, wkbGeometryType.wkbMultiLineString, null);
            lyr_water_line.CreateField(fld_level, 0);
            createCPG(_userFolder + mapName + @"\water_line.cpg", encoding);

            //LAKE
            DataSource water_area = drv.CreateDataSource(_userFolder + mapName + @"\water_area.shp", null);
            Layer lyr_water_area = water_area.CreateLayer("water_area", srs, wkbGeometryType.wkbMultiPolygon, null);
            lyr_water_area.CreateField(fld_level, 0);
            lyr_water_area.CreateField(fld_area, 0);
            createCPG(_userFolder + mapName + @"\water_area.cpg", encoding);

            // CITY
            DataSource citybig_point = drv.CreateDataSource(_userFolder + mapName + @"\citybig_point.shp", null);
            Layer lyr_citybig_point = citybig_point.CreateLayer("citybig_point", srs, wkbGeometryType.wkbPoint, null);
            lyr_citybig_point.CreateField(fld_level, 0);
            createCPG(_userFolder + mapName + @"\citybig_point.cpg", encoding);

            DataSource citymedium_point = drv.CreateDataSource(_userFolder + mapName + @"\citymedium_point.shp", null);
            Layer lyr_citymedium_point = citybig_point.CreateLayer("citymedium_point", srs, wkbGeometryType.wkbPoint, null);
            lyr_citymedium_point.CreateField(fld_level, 0);
            createCPG(_userFolder + mapName + @"\citymedium_point.cpg", encoding);

            DataSource citysmall_point = drv.CreateDataSource(_userFolder + mapName + @"\citysmall_point.shp", null);
            Layer lyr_citysmall_point = citysmall_point.CreateLayer("citysmall_point", srs, wkbGeometryType.wkbPoint, null);
            lyr_citysmall_point.CreateField(fld_level, 0);
            createCPG(_userFolder + mapName + @"\citysmall_point.cpg", encoding);

            DataSource cityverysmall_point = drv.CreateDataSource(_userFolder + mapName + @"\cityverysmall_point.shp", null);
            Layer lyr_cityverysmall_point = cityverysmall_point.CreateLayer("cityverysmall_point", srs, wkbGeometryType.wkbPoint, null);
            lyr_cityverysmall_point.CreateField(fld_level, 0);
            createCPG(_userFolder + mapName + @"\cityverysmall_point.cpg", encoding);

            // RESIDENTIAL
            DataSource city_area = drv.CreateDataSource(_userFolder + mapName + @"\city_area.shp", null);
            Layer lyr_city_area = city_area.CreateLayer("city_area", srs, wkbGeometryType.wkbPolygon, null);
            lyr_city_area.CreateField(fld_level, 0);
            lyr_city_area.CreateField(fld_area, 0);
            createCPG(_userFolder + mapName + @"\city_area.cpg", encoding);


            DataSource osm_ds = Ogr.Open(osmFile, 0);
            if (osm_ds == null)
            {
                MessageBox.Show("Error processing OSM data");
                return;
            }

            long nPoints=0, nLines= 0, nPolys = 0, nMultilines = 0;
  
            setStatus("Processing OSM ... ");
            _step++;
            string name,place,tags,highway,waterway,landuse,natural;

            // Use interleaved reading - http://www.gdal.org/drv_osm.html
            bool bHasLayersNonEmpty = false;
            do
            {
                bHasLayersNonEmpty = false;
                for (int iLayer = 0; iLayer < osm_ds.GetLayerCount(); iLayer++)
                {
                    Layer OGRLayer = osm_ds.GetLayerByIndex(iLayer);

                    Feature feat;
                    OSGeo.OGR.Feature nf = null;

                    while ((feat = OGRLayer.GetNextFeature()) != null)
                    {
                        bHasLayersNonEmpty = true;

                        try
                        {
                            Geometry geom = feat.GetGeometryRef();

                            if (!geom.Within(poly))
                                continue;

                            name = feat.GetFieldAsString("name");

//                            string faultyStr = Encoding.Default.GetString(Encoding.UTF8.GetBytes(name));
//                            name = faultyStr;

                            switch (iLayer)
                            {
                                case 0:
                                    nPoints++;
                                    if (nPoints % 10000 == 0)
                                        setStatus("Processing POINTS ... (" + Convert.ToString(nPoints) + ")");

                                    place = feat.GetFieldAsString("place");
                                    tags = feat.GetFieldAsString("other_tags");

                                    if (_useOSMRCity)
                                    {
                                        if (place == "city")
                                        {
                                            nf = new Feature(lyr_citybig_point.GetLayerDefn());
                                            nf.SetGeometry(geom.Intersection(poly));
                                            nf.SetField(0, name);
                                            lyr_citybig_point.CreateFeature(nf);
                                        }
                                        if (place == "town" && (_cityDetail == "VeyHigh" || _cityDetail == "High" || _cityDetail == "Medium"))
                                        {
                                            nf = new Feature(lyr_citymedium_point.GetLayerDefn());
                                            nf.SetGeometry(geom.Intersection(poly));
                                            nf.SetField(0, name);
                                            lyr_citymedium_point.CreateFeature(nf);
                                        }
                                        if (place == "town" && (_cityDetail == "Low"))
                                        {
                                            nf = new Feature(lyr_citysmall_point.GetLayerDefn());
                                            nf.SetGeometry(geom.Intersection(poly));
                                            nf.SetField(0, name);
                                            lyr_citysmall_point.CreateFeature(nf);
                                        }
                                        if (place == "village" && _cityDetail == "VeyHigh")
                                        {
                                            nf = new Feature(lyr_citysmall_point.GetLayerDefn());
                                            nf.SetGeometry(geom.Intersection(poly));
                                            nf.SetField(0, name);
                                            lyr_citysmall_point.CreateFeature(nf);
                                        }
                                        if (place == "village" && _cityDetail == "High")
                                        {
                                            nf = new Feature(lyr_cityverysmall_point.GetLayerDefn());
                                            nf.SetGeometry(geom.Intersection(poly));
                                            nf.SetField(0, name);
                                            lyr_cityverysmall_point.CreateFeature(nf);
                                        }
                                        nf = null;
                                    }
                                    break;
                                case 1:
                                    nLines++;
                                    if (nLines % 10000 == 0)
                                        setStatus("Processing LINES ... (" + Convert.ToString(nLines) + ")");
                                    //SetProgress((int)(cf * 100.0 / totF));

                                    highway = feat.GetFieldAsString("highway");
                                    waterway = feat.GetFieldAsString("waterway");
                                    tags = feat.GetFieldAsString("other_tags");

                                    if (_useOSMRoads)
                                    {
                                        if (highway == "motorway" || highway == "trunk")
                                        {
                                            nf = new Feature(lyr_roadbig_line.GetLayerDefn());
                                            nf.SetGeometry(geom.Intersection(poly).SimplifyPreserveTopology(_simplify / (3600 * 30)));
                                            nf.SetField(0, highway);
                                            lyr_roadbig_line.CreateFeature(nf);
                                        }
                                        if ( highway == "primary"   )
                                        {
                                            nf = new Feature(lyr_roadmedium_line.GetLayerDefn());
                                            nf.SetGeometry(geom.Intersection(poly).SimplifyPreserveTopology(_simplify / (3600 * 30)));
                                            nf.SetField(0, highway);
                                            lyr_roadmedium_line.CreateFeature(nf);
                                        }
                                        if (highway == "secondary" && (_roadsDetail == "High" || _roadsDetail == "VeyHigh"))
                                        {
                                            nf = new Feature(lyr_roadsmall_line.GetLayerDefn());
                                            nf.SetGeometry(geom.Intersection(poly).SimplifyPreserveTopology(_simplify / (3600 * 30)));
                                            nf.SetField(0, highway);
                                            lyr_roadsmall_line.CreateFeature(nf);
                                        }
                                        if (highway == "tertiary" && (_roadsDetail == "VeyHigh"))
                                        {
                                            nf = new Feature(lyr_roadsmall_line.GetLayerDefn());
                                            nf.SetGeometry(geom.Intersection(poly).SimplifyPreserveTopology(_simplify / (3600 * 30)));
                                            nf.SetField(0, highway);
                                            lyr_roadsmall_line.CreateFeature(nf);
                                        }


                                    }

                                    if (_useOSMRail)
                                    {
                                        if (tags.Contains("\"railway\"=>\"rail\""))
                                        {
                                            nf = new Feature(lyr_railroad_line.GetLayerDefn());
                                            nf.SetGeometry(geom.Intersection(poly).SimplifyPreserveTopology(_simplify / (3600 * 30)));
                                            nf.SetField(0, name);
                                            lyr_railroad_line.CreateFeature(nf);
                                        }
                                    }

                                    if (_useOSMRivers)
                                    {
                                        if (waterway == "river")
                                        {
                                            {
                                                nf = new Feature(lyr_water_line.GetLayerDefn());
                                                nf.SetGeometry(geom.Intersection(poly).SimplifyPreserveTopology(_simplify / (3600 * 30)));
                                                nf.SetField(0, "");
                                                lyr_water_line.CreateFeature(nf);
                                            }
                                        }
                                    }
                                    break;
                                case 2:
                                    nMultilines++;
                                    if (nMultilines % 10000 == 0)
                                        setStatus("Processing Multilines ... (" + Convert.ToString(nMultilines) + ")");
                                    break;
                                case 3:
                                    nPolys++;
                                    if (nPolys % 10000 == 0)
                                        setStatus("Processing POLYGON ... (" + Convert.ToString(nPolys) + ")");

                                    landuse = feat.GetFieldAsString("landuse");
                                    natural = feat.GetFieldAsString("natural");
                                    tags = feat.GetFieldAsString("other_tags");

                                    if (_useOSMRResidential)
                                    {
                                        if (landuse == "residential")
                                        {
                                            double area = geom.Area() * 12379.77;
                                            if (area > _citySize)
                                            {
                                                try
                                                {
                                                    nf = new Feature(lyr_city_area.GetLayerDefn());
                                                    nf.SetGeometry(geom.Simplify(_simplify / (3600 * 30)));
                                                    nf.SetField(0, name);
                                                    nf.SetField(1, area);
                                                    lyr_city_area.CreateFeature(nf);
                                                }
                                                catch
                                                {
                                                }

                                            }
                                        }
                                    }

                                    if (_useOSMRLakes)
                                    {
                                        if (natural == "water")
                                        {
                                            double area = geom.Area() * 12379.77;
                                            if (area > _lakesSize)
                                            {
                                                try
                                                {
                                                    nf = new Feature(lyr_water_area.GetLayerDefn());
                                                    nf.SetGeometry(geom.Intersection(poly).SimplifyPreserveTopology(_simplify / (3600 * 30)));
                                                    nf.SetField(0, name);
                                                    nf.SetField(1, area);
                                                    lyr_water_area.CreateFeature(nf);
                                                }
                                                catch
                                                {
                                                }
                                            }
                                        }

                                    }
                                    break;
                            }
                        }
                        catch
                        {

                        }
                    }
 
                }
            } while (bHasLayersNonEmpty);




            osm_ds.Dispose();
            osm_ds = null;

            roadbig_line.Dispose();
            roadbig_line = null;

            roadmedium_line.Dispose();
            roadmedium_line = null;

            roadsmall_line.Dispose();
            roadsmall_line = null;

            railroad_line.Dispose();
            railroad_line = null;

            water_line.Dispose();
            water_line = null;

            water_area.Dispose();
            water_area = null;

            citybig_point.Dispose();
            citybig_point = null;

            citymedium_point.Dispose();
            citymedium_point = null;

            citysmall_point.Dispose();
            citysmall_point = null;

            cityverysmall_point.Dispose();
            cityverysmall_point = null;


            city_area.Dispose();
            city_area = null;


           

        }

        private void createCPG(string filename, string encoding)
        {
            using (System.IO.StreamWriter file =
            new System.IO.StreamWriter(filename))
            {
                file.WriteLine(encoding);
            }
        }


        //private void processOSM_old(string osmFile, string mapName, double minLon, double minLat, double maxLon, double maxLat)
        //{
        //    // Configure OGR
        //    Gdal.SetConfigOption("OSM_MAX_TMPFILE_SIZE", "1024");
        //    Gdal.SetConfigOption("OGR_INTERLEAVED_READING", "YES");
        //    Gdal.SetConfigOption("OSM_COMPRESS_NODES", "YES");
        //    Gdal.SetConfigOption("CPL_TMPDIR", _tmpFolder);



        //    Ogr.RegisterAll();

        //    OSGeo.OGR.Driver drv = Ogr.GetDriverByName("ESRI Shapefile");
        //    OSGeo.OSR.SpatialReference srs = new OSGeo.OSR.SpatialReference(null);
        //    srs.ImportFromEPSG(4326);
        //    OSGeo.OGR.FieldDefn fld_level = new OSGeo.OGR.FieldDefn("LEVEL", FieldType.OFTString);
        //    OSGeo.OGR.Geometry ring = new OSGeo.OGR.Geometry(OSGeo.OGR.wkbGeometryType.wkbLinearRing);
        //    ring.AddPoint(minLon, minLat, 0);
        //    ring.AddPoint(minLon, maxLat, 0);
        //    ring.AddPoint(maxLon, maxLat, 0);
        //    ring.AddPoint(maxLon, minLat, 0);
        //    ring.AddPoint(minLon, minLat, 0);
        //    OSGeo.OGR.Geometry poly = new OSGeo.OGR.Geometry(OSGeo.OGR.wkbGeometryType.wkbPolygon);
        //    poly.AssignSpatialReference(srs);
        //    poly.AddGeometry(ring);


        //    //ROADS
        //    DataSource roadbig_line = drv.CreateDataSource(_userFolder + mapName + @"\roadbig_line.shp", null);
        //    Layer lyr_roadbig_line = roadbig_line.CreateLayer("roadbig_line", srs, wkbGeometryType.wkbLineString, null);
        //    lyr_roadbig_line.CreateField(fld_level, 0);

        //    DataSource roadmedium_line = drv.CreateDataSource(_userFolder + mapName + @"\roadmedium_line.shp", null);
        //    Layer lyr_roadmedium_line = roadbig_line.CreateLayer("roadmedium_line", srs, wkbGeometryType.wkbLineString, null);
        //    lyr_roadmedium_line.CreateField(fld_level, 0);

        //    DataSource roadsmall_line = drv.CreateDataSource(_userFolder + mapName + @"\roadsmall_line.shp", null);
        //    Layer lyr_roadsmall_line = roadsmall_line.CreateLayer("roadsmall_line", srs, wkbGeometryType.wkbLineString, null);
        //    lyr_roadsmall_line.CreateField(fld_level, 0);

        //    // RAILROAS
        //    DataSource railroad_line = drv.CreateDataSource(_userFolder + mapName + @"\railroad_line.shp", null);
        //    Layer lyr_railroad_line = railroad_line.CreateLayer("railroad_line", srs, wkbGeometryType.wkbLineString, null);
        //    lyr_railroad_line.CreateField(fld_level, 0);

        //    // RIVER
        //    DataSource water_line = drv.CreateDataSource(_userFolder + mapName + @"\water_line.shp", null);
        //    Layer lyr_water_line = water_line.CreateLayer("water_line", srs, wkbGeometryType.wkbMultiLineString, null);
        //    lyr_water_line.CreateField(fld_level, 0);

        //    //LAKE
        //    DataSource water_area = drv.CreateDataSource(_userFolder + mapName + @"\water_area.shp", null);
        //    Layer lyr_water_area = water_area.CreateLayer("water_area", srs, wkbGeometryType.wkbMultiPolygon, null);
        //    lyr_water_area.CreateField(fld_level, 0);


        //    // CITY
        //    DataSource citybig_point = drv.CreateDataSource(_userFolder + mapName + @"\citybig_point.shp", null);
        //    Layer lyr_citybig_point = citybig_point.CreateLayer("citybig_point", srs, wkbGeometryType.wkbPoint, null);
        //    lyr_citybig_point.CreateField(fld_level, 0);

        //    DataSource citymedium_point = drv.CreateDataSource(_userFolder + mapName + @"\citymedium_point.shp", null);
        //    Layer lyr_citymedium_point = citybig_point.CreateLayer("citymedium_point", srs, wkbGeometryType.wkbPoint, null);
        //    lyr_citymedium_point.CreateField(fld_level, 0);

        //    DataSource citysmall_point = drv.CreateDataSource(_userFolder + mapName + @"\citysmall_point.shp", null);
        //    Layer lyr_citysmall_point = citysmall_point.CreateLayer("citysmall_point", srs, wkbGeometryType.wkbPoint, null);
        //    lyr_citysmall_point.CreateField(fld_level, 0);

        //    DataSource cityverysmall_point = drv.CreateDataSource(_userFolder + mapName + @"\cityverysmall_point.shp", null);
        //    Layer lyr_cityverysmall_point = cityverysmall_point.CreateLayer("cityverysmall_point", srs, wkbGeometryType.wkbPoint, null);
        //    lyr_cityverysmall_point.CreateField(fld_level, 0);


        //    // RESIDENTIAL
        //    DataSource city_area = drv.CreateDataSource(_userFolder + mapName + @"\city_area.shp", null);
        //    Layer lyr_city_area = city_area.CreateLayer("city_area", srs, wkbGeometryType.wkbPolygon, null);
        //    lyr_city_area.CreateField(fld_level, 0);


        //    DataSource osm_ds = Ogr.Open(osmFile, 0);
        //    if (osm_ds == null)
        //    {
        //        MessageBox.Show("Error processing OSM data");
        //        return;
        //    }


        //    Layer layer;
        //    OSGeo.OGR.Feature f;
        //    long cf = 0;

        //    //            goto PP;
        //    //  ---------------------------------- POINTS      
        //    setStatus("Processing POINTS ... ");
        //    _step++;




        //    layer = osm_ds.GetLayerByIndex(0);
        //    layer.ResetReading();
        //    cf = 0;


        //    try
        //    {
        //        while ((f = layer.GetNextFeature()) != null)
        //        {
        //            cf++;
        //            if (cf % 10000 == 0)
        //                setStatus("Processing POINTS ... (" + Convert.ToString(cf) + ")");

        //            var geom = f.GetGeometryRef();
        //            string name = f.GetFieldAsString("name");
        //            string place = f.GetFieldAsString("place");
        //            string tags = f.GetFieldAsString("other_tags");

        //            if (!geom.Within(poly))
        //                continue;

        //            OSGeo.OGR.Feature nf = null;

        //            if (_useOSMRCity)
        //            {
        //                if (place == "city")
        //                {
        //                    nf = new Feature(lyr_citybig_point.GetLayerDefn());
        //                    nf.SetGeometry(geom.Intersection(poly));
        //                    nf.SetField(0, name);
        //                    lyr_citybig_point.CreateFeature(nf);
        //                }
        //                if (place == "town" && (_cityDetail == "High" || _cityDetail == "Medium"))
        //                {
        //                    nf = new Feature(lyr_citymedium_point.GetLayerDefn());
        //                    nf.SetGeometry(geom.Intersection(poly));
        //                    nf.SetField(0, name);
        //                    lyr_citymedium_point.CreateFeature(nf);
        //                }
        //                if (place == "town" && (_cityDetail == "Low"))
        //                {
        //                    nf = new Feature(lyr_citysmall_point.GetLayerDefn());
        //                    nf.SetGeometry(geom.Intersection(poly));
        //                    nf.SetField(0, name);
        //                    lyr_citysmall_point.CreateFeature(nf);
        //                }
        //                if (place == "village" && _cityDetail == "VeyHigh")
        //                {
        //                    nf = new Feature(lyr_citysmall_point.GetLayerDefn());
        //                    nf.SetGeometry(geom.Intersection(poly));
        //                    nf.SetField(0, name);
        //                    lyr_citysmall_point.CreateFeature(nf);
        //                }
        //                if (place == "village" && _cityDetail == "High")
        //                {
        //                    nf = new Feature(lyr_cityverysmall_point.GetLayerDefn());
        //                    nf.SetGeometry(geom.Intersection(poly));
        //                    nf.SetField(0, name);
        //                    lyr_cityverysmall_point.CreateFeature(nf);
        //                }


        //                nf = null;
        //            }

        //        }
        //    }
        //    catch
        //    {
        //    }


        //    //  ---------------------------------- LINES  
        //    setStatus("Processing LINES ... ");
        //    _step++;

        //    layer = osm_ds.GetLayerByIndex(1);
        //    layer.ResetReading();
        //    cf = 0;
        //    //            try
        //    //           {

        //    while ((f = layer.GetNextFeature()) != null)
        //    {
        //        cf++;
        //        if (cf % 10000 == 0)
        //            setStatus("Processing LINES ... (" + Convert.ToString(cf) + ")");
        //        //SetProgress((int)(cf * 100.0 / totF));

        //        var geom = f.GetGeometryRef();
        //        string name = f.GetFieldAsString("name");
        //        string highway = f.GetFieldAsString("highway");
        //        string waterway = f.GetFieldAsString("waterway");
        //        string tags = f.GetFieldAsString("other_tags");

        //        OSGeo.OGR.Feature nf = null;

        //        if (_useOSMRoads)
        //        {
        //            if (highway == "motorway" || highway == "trunk")
        //            {
        //                nf = new Feature(lyr_roadbig_line.GetLayerDefn());
        //                nf.SetGeometry(geom.Intersection(poly).SimplifyPreserveTopology(_simplify / (3600 * 30)));
        //                nf.SetField(0, name);
        //                lyr_roadbig_line.CreateFeature(nf);
        //            }
        //            if (_roadsDetail == "High" && highway == "primary")
        //            {
        //                nf = new Feature(lyr_roadmedium_line.GetLayerDefn());
        //                nf.SetGeometry(geom.Intersection(poly).SimplifyPreserveTopology(_simplify / (3600 * 30)));
        //                nf.SetField(0, name);
        //                lyr_roadmedium_line.CreateFeature(nf);
        //            }
        //            if (highway == "secondary" || highway == "tertiary" || (_roadsDetail != "High" && highway == "primary"))
        //            {
        //                nf = new Feature(lyr_roadsmall_line.GetLayerDefn());
        //                nf.SetGeometry(geom.Intersection(poly).SimplifyPreserveTopology(_simplify / (3600 * 30)));
        //                nf.SetField(0, name);
        //                lyr_roadsmall_line.CreateFeature(nf);
        //            }
        //        }

        //        if (_useOSMRail)
        //        {
        //            if (tags.Contains("\"railway\"=>\"rail\""))
        //            {
        //                nf = new Feature(lyr_railroad_line.GetLayerDefn());
        //                nf.SetGeometry(geom.Intersection(poly).SimplifyPreserveTopology(_simplify / (3600 * 30)));
        //                nf.SetField(0, name);
        //                lyr_railroad_line.CreateFeature(nf);
        //            }
        //        }

        //        if (_useOSMRivers)
        //        {
        //            if (waterway == "river")
        //            {
        //                //double l = geom.Length();
        //                //bool ok = false;
        //                //if (_riverDetail == " High")
        //                //    ok = (l > 0);
        //                //if (_riverDetail == "Medium")
        //                //    ok = (l > 0.1);
        //                //if (_riverDetail == "Low")
        //                //    ok = (l > 0.2);
        //                //if (ok)
        //                {
        //                    nf = new Feature(lyr_water_line.GetLayerDefn());
        //                    nf.SetGeometry(geom.Intersection(poly).SimplifyPreserveTopology(_simplify / (3600 * 30)));
        //                    nf.SetField(0, "");
        //                    lyr_water_line.CreateFeature(nf);
        //                }
        //            }
        //        }

        //    }
        //    //            }
        //    //            catch
        //    //            {
        //    //            }



        //    //  ---------------------------------- MULTILINESTRING      
        //    setStatus("Processing MULTILINESTRING ... ");
        //    _step++;

        //    layer = osm_ds.GetLayerByIndex(2);
        //    layer.ResetReading();
        //    cf = 0;
        //    while ((f = layer.GetNextFeature()) != null)
        //    {
        //        cf++;
        //        if (cf % 10000 == 0)
        //            setStatus("Processing POLYGON ... (" + Convert.ToString(cf) + ")");
        //    }


        //    //  ---------------------------------- POLYGON      
        //    setStatus("Processing POLYGON ... ");
        //    _step++;

        //    layer = osm_ds.GetLayerByIndex(3);
        //    layer.ResetReading();
        //    cf = 0;



        //    try
        //    {
        //        while ((f = layer.GetNextFeature()) != null)
        //        {
        //            cf++;
        //            if (cf % 10000 == 0)
        //                setStatus("Processing POLYGON ... (" + Convert.ToString(cf) + ")");

        //            var geom = f.GetGeometryRef();
        //            string name = f.GetFieldAsString("name");
        //            string landuse = f.GetFieldAsString("landuse");
        //            string natural = f.GetFieldAsString("natural");
        //            string tags = f.GetFieldAsString("other_tags");
        //            OSGeo.OGR.Feature nf = null;

        //            if (!geom.Within(poly))
        //                continue;

        //            if (_useOSMRResidential)
        //            {
        //                if (landuse == "residential")
        //                {
        //                    double area = geom.Area() * 12379.77;
        //                    if (area > _citySize)
        //                    {
        //                        try
        //                        {
        //                            nf = new Feature(lyr_city_area.GetLayerDefn());
        //                            nf.SetGeometry(geom.Simplify(_simplify / (3600 * 30)));
        //                            nf.SetField(0, area.ToString(CultureInfo.InvariantCulture));
        //                            lyr_city_area.CreateFeature(nf);
        //                        }
        //                        catch
        //                        {
        //                        }

        //                    }
        //                }
        //            }

        //            if (_useOSMRLakes)
        //            {
        //                if (natural == "water")
        //                {
        //                    double area = geom.Area() * 12379.77;
        //                    if (area > _lakesSize)
        //                    {
        //                        try
        //                        {
        //                            nf = new Feature(lyr_water_area.GetLayerDefn());
        //                            nf.SetGeometry(geom.Intersection(poly).SimplifyPreserveTopology(_simplify / (3600 * 30)));
        //                            nf.SetField(0, name);
        //                            lyr_water_area.CreateFeature(nf);
        //                        }
        //                        catch
        //                        {
        //                        }
        //                    }
        //                }

        //            }

        //        }
        //    }
        //    catch
        //    {
        //    }

        //    osm_ds.Dispose();
        //    osm_ds = null;

        //    roadbig_line.Dispose();
        //    roadbig_line = null;

        //    roadmedium_line.Dispose();
        //    roadmedium_line = null;

        //    roadsmall_line.Dispose();
        //    roadsmall_line = null;

        //    railroad_line.Dispose();
        //    railroad_line = null;

        //    water_line.Dispose();
        //    water_line = null;

        //    water_area.Dispose();
        //    water_area = null;

        //    citybig_point.Dispose();
        //    citybig_point = null;

        //    citymedium_point.Dispose();
        //    citymedium_point = null;

        //    citysmall_point.Dispose();
        //    citysmall_point = null;

        //    cityverysmall_point.Dispose();
        //    cityverysmall_point = null;


        //    city_area.Dispose();
        //    city_area = null;




        //}



        private void backgroundWorkerLKM_DoWork(object sender, DoWorkEventArgs e)
        {
            // http://www.overpass-api.de/api/xapi?way[bbox=11,42,14,44][highway=motorway|trunk|primary|secondary]
            // [natural=water]
            // [railway=rail]
            // [waterway=river|waterway=canal]



            _step = 0; // Download OSM
            _nTiles = 1;
            NSTEPS = 4;

            string vmapFile = vmapFile = GDALUtil.GetVMAPArea(_minLon, _minLat, _maxLon, _maxLat); ;
            string osmFile = GDALUtil.GetVMAPArea(_minLon, _minLat, _maxLon, _maxLat);


            bytes_total = (long) ( 45000000 * ((_maxLon - _minLon) * (_maxLat - _minLat)) / 6 );


            string bbox = _minLon.ToString(CultureInfo.InvariantCulture) + "," + _minLat.ToString(CultureInfo.InvariantCulture) + "," + 
                _maxLon.ToString(CultureInfo.InvariantCulture) + "," + _maxLat.ToString(CultureInfo.InvariantCulture) ;
            
            string api_url;

            

            //Roads
            if (_useOSMRoads)
            {
                _step++;

                api_url = "http://www.overpass-api.de/api/interpreter?data=[bbox][maxsize:1073741824][out:xml][timeout:900];";
                api_url += "(";

                api_url+= "way[\"highway\"~\"motorway|trunk";
                if (_roadsDetail == "Medium")
                    api_url += "|primary";
                if (_roadsDetail == "High")
                    api_url += "|primary|secondary";
                if (_roadsDetail == "VeyHigh")
                    api_url += "|tertiary";
                api_url += "\"];";
                api_url += ");";
                api_url += "(._;>;);out;&bbox=" + bbox;


                if (!checkAPIStatus())
                    return;

                downloadOSM(System.Uri.EscapeUriString(api_url), "highway.osm", _osmFolder);
                processRoadsOSM(_osmFolder + "highway.osm", _mapName);

                _step++;
                api_url = "http://www.overpass-api.de/api/interpreter?data=[bbox][maxsize:1073741824][out:xml][timeout:900];";
                api_url += "(";
                api_url += "way[\"railway\"~\"rail]"; 
                api_url += ");";
                api_url += "(._;>;);out;&bbox=" + bbox;

                downloadOSM(System.Uri.EscapeUriString(api_url), "railway.osm", _osmFolder);
                processRailwayOSM(_osmFolder + "railway.osm", _mapName);


                
            }
            else
            {   _step++;
                downloadPlanetOSM(osmFile, _osmFolder);
                _step++;
                processRoadsPlanetOSM(_osmFolder + osmFile + ".zip/roads.shp", _mapName, _minLon, _minLat, _maxLon, _maxLat);
            }

            //Rivers
            if ( _useOSMRivers ) 
            {
                _step++;

                api_url = "http://www.overpass-api.de/api/interpreter?data=[bbox][maxsize:1073741824][out:xml][timeout:900];";
                api_url += "(";
                if (_riverDetail == "High")
                    api_url += "way[\"waterway\"~\"river\"];";
                else
                    api_url += "rel[\"waterway\"~\"river\"];";
                api_url += ");";
                api_url += "(._;>;);out;&bbox=" + bbox;


                if (!checkAPIStatus())
                    return;
                downloadOSM(System.Uri.EscapeUriString(api_url), "rivers.osm", _osmFolder);
                _step++;
                processRiverOSM(_osmFolder + "rivers.osm", _mapName);


                
            }
            else
            {
                _step++;
                downloadVMAP(vmapFile, _vmapFolder);
                _step++;
                processRiversVMAP(_vmapFolder + vmapFile + ".zip/hydro-water-course-l.shp", _mapName, _minLon, _minLat, _maxLon, _maxLat);

            }


            //Lakes
            if (_useOSMRLakes)
            {
                _step++;


                api_url = "http://www.overpass-api.de/api/interpreter?data=[bbox][maxsize:1073741824][out:xml][timeout:900];";
                api_url += "(";

                api_url += "rel[\"natural\"~\"water\"];";
                api_url += ");";
                api_url += "(._;>;);out;&bbox=" + bbox;

                downloadOSM(System.Uri.EscapeUriString(api_url), "water.osm", _osmFolder);
                _step++;
                processWaterOSM(_osmFolder + "water.osm", _mapName);


               

            }
            else
            {
                _step++;
                downloadLakesVMAP(vmapFile, _vmapFolder);
                _step++;
                processLakesVMAP(_vmapFolder + vmapFile + ".zip/hydro-inland-water-a.shp", _mapName, _minLon, _minLat, _maxLon, _maxLat);
            }



            // City Area
            if (_useOSMRResidential)
            {
                _step++;

                api_url = "http://www.overpass-api.de/api/interpreter?data=[bbox][maxsize:1073741824][out:xml][timeout:900];";
                api_url += "(";
                api_url += "way[\"landuse\"~\"residential\"];";
                api_url += ");";
                api_url += "(._;>;);out;&bbox=" + bbox;

                downloadOSM(System.Uri.EscapeUriString(api_url), "residential.osm", _osmFolder);
                _step++;
                processResidentialOSM(_osmFolder + "residential.osm", _mapName);


                _step++;

                api_url = "http://www.overpass-api.de/api/interpreter?data=[bbox][maxsize:1073741824][out:xml][timeout:900];";
                api_url += "(";
                api_url += "node[\"place\"~\"^(city|town|village)$\"];";
                api_url += ");";
                api_url += "(._;>;);out;&bbox=" + bbox;

                downloadOSM(System.Uri.EscapeUriString(api_url), "city.osm", _osmFolder);
                _step++;
                processCityOSM(_osmFolder + "city.osm", _mapName);




            }
            else
            {
                _step++;
                downloadVMAP(vmapFile, _vmapFolder);
                _step++;
                processCityAreasVMAP(_vmapFolder + vmapFile + ".zip/pop-built-up-a.shp", _mapName, _minLon, _minLat, _maxLon, _maxLat);

                // City Points
                _step++;
                downloadVMAP(vmapFile, _vmapFolder);
                _step++;
                processUrbanPlanetOSM(_osmFolder + osmFile + ".zip/urban.shp", _mapName, _minLon, _minLat, _maxLon, _maxLat);
            }




            //Ocean
            //_step++;
            //downloadVMAP(vmapFile, _vmapFolder);
            //_step++;
            //processOceanVMAP(_vmapFolder + vmapFile + ".zip/bnd-ocean-a.shp", _mapName, _minLon, _minLat, _maxLon, _maxLat);


            processOcean(_mapName, _minLon, _minLat, _maxLon, _maxLat);


            _step++;
            createTBLFile( _mapName);

            _step++;
            //createFinalZip(_mapName, _outFolder);


            _step = NSTEPS;
            SetProgress(100);


            MessageBox.Show("Done");

        }

        private void processOcean(string mapName, double minLon, double minLat, double maxLon, double maxLat)
        {
            setStatus("Processing Ocean .. ");

            Ogr.RegisterAll();
            OSGeo.OGR.Driver drv = Ogr.GetDriverByName("ESRI Shapefile");
            OSGeo.OSR.SpatialReference srs = new OSGeo.OSR.SpatialReference(null);
            srs.ImportFromEPSG(4326);
            OSGeo.OGR.FieldDefn fld_level = new OSGeo.OGR.FieldDefn("LEVEL", FieldType.OFTString);


            OSGeo.OGR.Geometry ring = new OSGeo.OGR.Geometry(OSGeo.OGR.wkbGeometryType.wkbLinearRing);
            ring.AddPoint(minLon, minLat, 0);
            ring.AddPoint(minLon, maxLat, 0);
            ring.AddPoint(maxLon, maxLat, 0);
            ring.AddPoint(maxLon, minLat, 0);
            ring.AddPoint(minLon, minLat, 0);
            OSGeo.OGR.Geometry poly = new OSGeo.OGR.Geometry(OSGeo.OGR.wkbGeometryType.wkbPolygon);
            poly.AddGeometry(ring);

            DataSource bnd_ocean_a = drv.CreateDataSource(_userFolder + mapName + @"\coast_area.shp", null);
            Layer lyr_bnd_ocean_a = bnd_ocean_a.CreateLayer("bnd_ocean_a", srs, wkbGeometryType.wkbPolygon, null);
            lyr_bnd_ocean_a.CreateField(fld_level, 0);

            DataSource osm_ds = Ogr.Open("/vsizip/" +  Path.GetDirectoryName(Application.ExecutablePath) +  "/data.zip/coast/coast_area.shp", 0);
            if (osm_ds == null)
            {
                MessageBox.Show("Error processing costal");
                return;
            }

            Layer layer = osm_ds.GetLayerByIndex(0);

            if (layer == null)
            {
                MessageBox.Show("Installation error. Please reinstall the software");
                return;
            }

            int n = osm_ds.GetLayerCount();


            layer = osm_ds.GetLayerByIndex(0);

            OSGeo.OGR.Feature f;
            layer.ResetReading();
            long cf = 0;
            long totF = layer.GetFeatureCount(0);

            try
            {
                
                while (cf < totF )
                {
                    f = layer.GetNextFeature();
                    if (f == null)
                        break;

                    cf++;
                    SetProgress((int)(cf * 100.0 / totF));

                    var geom = f.GetGeometryRef();
                    String name = f.GetFieldAsString(1);


                    OSGeo.OGR.Feature nf = null;

                    nf = new Feature(lyr_bnd_ocean_a.GetLayerDefn());
                    //Envelope env = new Envelope();
                    //geom.GetEnvelope(env) ;

                    try
                    {
                        nf.SetGeometry(geom.Intersection(poly));
                    }
                    catch
                    {
                    }
                    nf.SetField(0, "");
                    lyr_bnd_ocean_a.CreateFeature(nf);

                    nf = null;
                }
            }
            catch
            {
            }


            osm_ds.Dispose();
            osm_ds = null;

            bnd_ocean_a.Dispose();
            bnd_ocean_a = null;
        }

        private void processCityOSM(string osmFile, string mapName)
        {
            setStatus("Processing City points .. ");

            Ogr.RegisterAll();
            OSGeo.OGR.Driver drv = Ogr.GetDriverByName("ESRI Shapefile");
            OSGeo.OSR.SpatialReference srs = new OSGeo.OSR.SpatialReference(null);
            srs.ImportFromEPSG(4326);
            OSGeo.OGR.FieldDefn fld_level = new OSGeo.OGR.FieldDefn("LEVEL", FieldType.OFTString);


            DataSource roadbig_line = drv.CreateDataSource(_userFolder + mapName + @"\citybig_point.shp", null);
            Layer lyr_roadbig_line = roadbig_line.CreateLayer("citybig_point", srs, wkbGeometryType.wkbPoint, null);
            lyr_roadbig_line.CreateField(fld_level, 0);

            DataSource roadmedium_line = drv.CreateDataSource(_userFolder + mapName + @"\citymedium_point.shp", null);
            Layer lyr_roadmedium_line = roadbig_line.CreateLayer("citymedium_point", srs, wkbGeometryType.wkbPoint, null);
            lyr_roadmedium_line.CreateField(fld_level, 0);

            DataSource roadsmall_line = drv.CreateDataSource(_userFolder + mapName + @"\citysmall_point.shp", null);
            Layer lyr_roadsmall_line = roadsmall_line.CreateLayer("citysmall_point", srs, wkbGeometryType.wkbPoint, null);
            lyr_roadsmall_line.CreateField(fld_level, 0);

            DataSource railroad_line = drv.CreateDataSource(_userFolder + mapName + @"\cityverysmall_point.shp", null);
            Layer lyr_railroad_line = railroad_line.CreateLayer("cityverysmall_point", srs, wkbGeometryType.wkbPoint, null);
            lyr_railroad_line.CreateField(fld_level, 0);


            DataSource osm_ds = Ogr.Open( osmFile, 0);
            if (osm_ds == null)
            {
                MessageBox.Show("Error processing City points");
                return;
            }

            Layer layer = osm_ds.GetLayerByIndex(0);

            if (layer == null)
            {
                MessageBox.Show("Installation error. Please reinstall the software");
                return;
            }

            int n = osm_ds.GetLayerCount();



            OSGeo.OGR.Feature f;
            layer.ResetReading();
            long cf = 0;
            long totF = layer.GetFeatureCount(0);

            while ((f = layer.GetNextFeature()) != null)
            {
                cf++;
                SetProgress((int)(cf * 100.0 / totF));

                var geom = f.GetGeometryRef();
                string name = f.GetFieldAsString(1);
                string level = f.GetFieldAsString(7);

                OSGeo.OGR.Feature nf = null;
                if (level == "city")
                {
                    nf = new Feature(lyr_roadbig_line.GetLayerDefn());
                    nf.SetGeometry(geom.Clone());
                    nf.SetField(0, name);
                    lyr_roadbig_line.CreateFeature(nf);
                }
                else if (level == "town")
                {
                    nf = new Feature(lyr_roadmedium_line.GetLayerDefn());
                    nf.SetGeometry(geom.Clone());
                    nf.SetField(0, name);
                    lyr_roadmedium_line.CreateFeature(nf);
                }
                else if (level == "village")
                {
                    nf = new Feature(lyr_roadsmall_line.GetLayerDefn());
                    nf.SetGeometry(geom.Clone());
                    nf.SetField(0, name);
                    lyr_roadsmall_line.CreateFeature(nf);
                }
                else if (level == "3")
                {
                    nf = new Feature(lyr_railroad_line.GetLayerDefn());
                    nf.SetGeometry(geom.Clone());
                    nf.SetField(0, name);
                    lyr_railroad_line.CreateFeature(nf);
                }
                nf = null;
            }


            osm_ds.Dispose();
            osm_ds = null;

            roadbig_line.Dispose();
            roadbig_line = null;

            roadmedium_line.Dispose();
            roadmedium_line = null;

            roadsmall_line.Dispose();
            roadsmall_line = null;

            railroad_line.Dispose();
            railroad_line = null;
        }

        private void processResidentialOSM(string osmFile, string mapName)
        {
            setStatus("Processing City areas .. ");

            Ogr.RegisterAll();
            OSGeo.OGR.Driver drv = Ogr.GetDriverByName("ESRI Shapefile");
            OSGeo.OSR.SpatialReference srs = new OSGeo.OSR.SpatialReference(null);
            srs.ImportFromEPSG(4326);
            OSGeo.OGR.FieldDefn fld_level = new OSGeo.OGR.FieldDefn("LEVEL", FieldType.OFTString);



            DataSource dataSource = drv.CreateDataSource(_userFolder + mapName + @"\city_area.shp", null);
            Layer layerSource = dataSource.CreateLayer("city_area", srs, wkbGeometryType.wkbPolygon, null);
            layerSource.CreateField(fld_level, 0);

            DataSource osm_ds = Ogr.Open(osmFile, 0);
            if (osm_ds == null)
            {
                MessageBox.Show("Error processing Roads");
                return;
            }

            Layer layer = osm_ds.GetLayerByIndex(2);

            if (layer == null)
            {
                MessageBox.Show("Installation error. Please reinstall the software");
                return;
            }

            int n = osm_ds.GetLayerCount();


            OSGeo.OGR.Feature f;
            layer.ResetReading();
            long cf = 0;
            long totF = layer.GetFeatureCount(0);

            while ((f = layer.GetNextFeature()) != null)
            {
                cf++;
                SetProgress((int)(cf * 100.0 / totF));

                var geom = f.GetGeometryRef();
                String name = f.GetFieldAsString(1);


                OSGeo.OGR.Feature nf = null;

                nf = new Feature(layerSource.GetLayerDefn());
                //Envelope env = new Envelope();
                //geom.GetEnvelope(env) ;

                try
                {
                    nf.SetGeometry(geom.SimplifyPreserveTopology(_simplify / (3600 * 30)));
                }
                catch
                {
                }
                nf.SetField(0, "");
                layerSource.CreateFeature(nf);

                nf = null;
            }


            osm_ds.Dispose();
            osm_ds = null;

            dataSource.Dispose();
            dataSource = null;
        }

        private bool checkAPIStatus()
        {

            var webRequest = WebRequest.Create(@"http://www.overpass-api.de/api/status");
            string strContent;
            using (var response = webRequest.GetResponse())
            using (var content = response.GetResponseStream())
            using (var reader = new StreamReader(content))
            {
                strContent = reader.ReadToEnd();
            }

            if (!strContent.Contains(" slots available now.\n"))
            {
                MessageBox.Show("No Slot avalable at the moment on OSM Server\n\n" + strContent + "\nRetry later");
                return false;
            }
            return true;
        }


        private void createFinalZipNew(string mapName, string outFolder)
        {

            setStatus("Creting LKM file ... ");

            string startPath = _userFolder + mapName + "\\";
            string zipPath = outFolder + @"/" + mapName + ".LKM";

            if (File.Exists(zipPath) )
            {
                File.Delete(zipPath);
            }

            using (ZipArchive zip = ZipFile.Open(zipPath, ZipArchiveMode.Create))
            {
                DirectoryInfo f = new DirectoryInfo(startPath);
                FileInfo[] a = f.GetFiles();
                for (int i = 0; i < a.Length; i++)
                {
                    setStatus("Adding  " + a[i].Name);
                    zip.CreateEntryFromFile(startPath + a[i].Name, a[i].Name);
                }
                

                
            }
            setStatus("Done");
        }



 /*       private void createFinalZip(string mapName, string outFolder)
        {
            
            setStatus("Creting LKM file ... ");

            string startPath = _userFolder + mapName + "\\";
            string zipPath = outFolder + @"/" + mapName + ".LKM";

            Directory.SetCurrentDirectory(startPath);
            using (ZipFile zip = new ZipFile(zipPath))
            {
                try
                {

                    DirectoryInfo f = new DirectoryInfo(startPath);
                    FileInfo[] a = f.GetFiles();
                    for (int i = 0; i < a.Length; i++)
                    {
                        setStatus("Adding  " + a[i].Name );
                        zip.AddFile(startPath + a[i].Name,"");
                    }
                    zip.Save();
                }
                catch
                {
                }
                setStatus("Done ");
            
            }

        }*/

        private void createTBLFile( string mapName)
        {

            setStatus("Creating TBL file ... ");

            string path = _userFolder + mapName + "\\topology.tpl";

            using (StreamWriter sw = new StreamWriter(path))
            {
                sw.WriteLine("coast_area,  5005,,,64,96,240");
                sw.WriteLine("water_area, 5010,,1,64,96,240");
                sw.WriteLine("water_line, 5020,,,64,96,240");
                sw.WriteLine("city_area,  5110,,,223,223,0");
                sw.WriteLine("roadbig_line, 5030,,,240,64,64");
                sw.WriteLine("roadmedium_line, 5040,,,240,64,64");
                sw.WriteLine("roadsmall_line, 5050,,,240,64,64");
                sw.WriteLine("railroad_line, 5060,,,64,64,64");
                sw.WriteLine("citybig_point, 5070,218,1,223,223,0");
                sw.WriteLine("citymedium_point, 5080,501,1,223,223,0");
                sw.WriteLine("citysmall_point, 5090,502,1,223,223,0");
            }	
        }

        private void processUrbanPlanetOSM(string osmFile, string mapName, double minLon, double minLat, double maxLon, double maxLat)
        {
            setStatus("Processing City points .. ");

            Ogr.RegisterAll();
            OSGeo.OGR.Driver drv = Ogr.GetDriverByName("ESRI Shapefile");
            OSGeo.OSR.SpatialReference srs = new OSGeo.OSR.SpatialReference(null);
            srs.ImportFromEPSG(4326);
            OSGeo.OGR.FieldDefn fld_level = new OSGeo.OGR.FieldDefn("LEVEL", FieldType.OFTString);


            OSGeo.OGR.Geometry ring = new OSGeo.OGR.Geometry(OSGeo.OGR.wkbGeometryType.wkbLinearRing);
            ring.AddPoint(minLon, minLat, 0);
            ring.AddPoint(minLon, maxLat, 0);
            ring.AddPoint(maxLon, maxLat, 0);
            ring.AddPoint(maxLon, minLat, 0);
            ring.AddPoint(minLon, minLat, 0);
            OSGeo.OGR.Geometry poly = new OSGeo.OGR.Geometry(OSGeo.OGR.wkbGeometryType.wkbPolygon);
            poly.AddGeometry(ring);

            DataSource roadbig_line = drv.CreateDataSource(_userFolder + mapName + @"\citybig_point.shp", null);
            Layer lyr_roadbig_line = roadbig_line.CreateLayer("citybig_point", srs, wkbGeometryType.wkbPoint, null);
            lyr_roadbig_line.CreateField(fld_level, 0);

            DataSource roadmedium_line = drv.CreateDataSource(_userFolder + mapName + @"\citymedium_point.shp", null);
            Layer lyr_roadmedium_line = roadbig_line.CreateLayer("citymedium_point", srs, wkbGeometryType.wkbPoint, null);
            lyr_roadmedium_line.CreateField(fld_level, 0);

            DataSource roadsmall_line = drv.CreateDataSource(_userFolder + mapName + @"\citysmall_point.shp", null);
            Layer lyr_roadsmall_line = roadsmall_line.CreateLayer("citysmall_point", srs, wkbGeometryType.wkbPoint, null);
            lyr_roadsmall_line.CreateField(fld_level, 0);

            DataSource railroad_line = drv.CreateDataSource(_userFolder + mapName + @"\cityverysmall_point.shp", null);
            Layer lyr_railroad_line = railroad_line.CreateLayer("cityverysmall_point", srs, wkbGeometryType.wkbPoint, null);
            lyr_railroad_line.CreateField(fld_level, 0);


            DataSource osm_ds = Ogr.Open("/vsizip/" + osmFile, 0);
            if (osm_ds == null)
            {
                MessageBox.Show("Error processing Roads");
                return;
            }

            Layer layer = osm_ds.GetLayerByIndex(0);

            if (layer == null)
            {
                MessageBox.Show("Installation error. Please reinstall the software");
                return;
            }

            int n = osm_ds.GetLayerCount();


            layer = osm_ds.GetLayerByIndex(0);

            OSGeo.OGR.Feature f;
            layer.ResetReading();
            long cf = 0;
            long totF = layer.GetFeatureCount(0);

            while ((f = layer.GetNextFeature()) != null)
            {
                cf++;
                SetProgress((int)(cf * 100.0 / totF));

                var geom = f.GetGeometryRef();
                string name = f.GetFieldAsString(1);
                string level = f.GetFieldAsString(2);

                OSGeo.OGR.Feature nf = null;
                if (level == "-1")
                {
                    nf = new Feature(lyr_roadbig_line.GetLayerDefn());
                    nf.SetGeometry(geom.Intersection(poly));
                    nf.SetField(0, name);
                    lyr_roadbig_line.CreateFeature(nf);
                }
                else if (level == "1")
                {
                    nf = new Feature(lyr_roadmedium_line.GetLayerDefn());
                    nf.SetGeometry(geom.Intersection(poly));
                    nf.SetField(0, name);
                    lyr_roadmedium_line.CreateFeature(nf);
                }
                else if (level == "2")
                {
                    nf = new Feature(lyr_roadsmall_line.GetLayerDefn());
                    nf.SetGeometry(geom.Intersection(poly));
                    nf.SetField(0, name);
                    lyr_roadsmall_line.CreateFeature(nf);
                }
                else if (level == "3")
                {
                    nf = new Feature(lyr_railroad_line.GetLayerDefn());
                    nf.SetGeometry(geom.Intersection(poly));
                    nf.SetField(0, name);
                    lyr_railroad_line.CreateFeature(nf);
                }
                nf = null;
            }


            osm_ds.Dispose();
            osm_ds = null;

            roadbig_line.Dispose();
            roadbig_line = null;

            roadmedium_line.Dispose();
            roadmedium_line = null;

            roadsmall_line.Dispose();
            roadsmall_line = null;

            railroad_line.Dispose();
            railroad_line = null;
        }


        private void processCityAreasVMAP(string vmapFolder, string mapName, double minLon, double minLat, double maxLon, double maxLat)
        {
            setStatus("Processing City areas .. ");

            Ogr.RegisterAll();
            OSGeo.OGR.Driver drv = Ogr.GetDriverByName("ESRI Shapefile");
            OSGeo.OSR.SpatialReference srs = new OSGeo.OSR.SpatialReference(null);
            srs.ImportFromEPSG(4326);
            OSGeo.OGR.FieldDefn fld_level = new OSGeo.OGR.FieldDefn("LEVEL", FieldType.OFTString);


            OSGeo.OGR.Geometry ring = new OSGeo.OGR.Geometry(OSGeo.OGR.wkbGeometryType.wkbLinearRing);
            ring.AddPoint(minLon, minLat, 0);
            ring.AddPoint(minLon, maxLat, 0);
            ring.AddPoint(maxLon, maxLat, 0);
            ring.AddPoint(maxLon, minLat, 0);
            ring.AddPoint(minLon, minLat, 0);
            OSGeo.OGR.Geometry poly = new OSGeo.OGR.Geometry(OSGeo.OGR.wkbGeometryType.wkbPolygon);
            poly.AddGeometry(ring);

            DataSource dataSource = drv.CreateDataSource(_userFolder + mapName + @"\city_area.shp", null);
            Layer layerSource = dataSource.CreateLayer("city_area", srs, wkbGeometryType.wkbPolygon, null);
            layerSource.CreateField(fld_level, 0);

            DataSource osm_ds = Ogr.Open("/vsizip/" + vmapFolder, 0);
            if (osm_ds == null)
            {
                MessageBox.Show("Error processing Roads");
                return;
            }

            Layer layer = osm_ds.GetLayerByIndex(0);

            if (layer == null)
            {
                MessageBox.Show("Installation error. Please reinstall the software");
                return;
            }

            int n = osm_ds.GetLayerCount();


            layer = osm_ds.GetLayerByIndex(0);

            OSGeo.OGR.Feature f;
            layer.ResetReading();
            long cf = 0;
            long totF = layer.GetFeatureCount(0);

            while ((f = layer.GetNextFeature()) != null)
            {
                cf++;
                SetProgress((int)(cf * 100.0 / totF));

                var geom = f.GetGeometryRef();
                String name = f.GetFieldAsString(1);


                OSGeo.OGR.Feature nf = null;

                nf = new Feature(layerSource.GetLayerDefn());
                //Envelope env = new Envelope();
                //geom.GetEnvelope(env) ;

                try
                {
                    nf.SetGeometry(geom.Intersection(poly));
                }
                catch
                {
                }
                nf.SetField(0, "");
                layerSource.CreateFeature(nf);

                nf = null;
            }


            osm_ds.Dispose();
            osm_ds = null;

            dataSource.Dispose();
            dataSource = null;
        }


        private void processOceanVMAP(string vmapFolder, string mapName, double minLon, double minLat, double maxLon, double maxLat)
        {
            setStatus("Processing Ocean .. ");

            Ogr.RegisterAll();
            OSGeo.OGR.Driver drv = Ogr.GetDriverByName("ESRI Shapefile");
            OSGeo.OSR.SpatialReference srs = new OSGeo.OSR.SpatialReference(null);
            srs.ImportFromEPSG(4326);
            OSGeo.OGR.FieldDefn fld_level = new OSGeo.OGR.FieldDefn("LEVEL", FieldType.OFTString);


            OSGeo.OGR.Geometry ring = new OSGeo.OGR.Geometry(OSGeo.OGR.wkbGeometryType.wkbLinearRing);
            ring.AddPoint(minLon, minLat, 0);
            ring.AddPoint(minLon, maxLat, 0);
            ring.AddPoint(maxLon, maxLat, 0);
            ring.AddPoint(maxLon, minLat, 0);
            ring.AddPoint(minLon, minLat, 0);
            OSGeo.OGR.Geometry poly = new OSGeo.OGR.Geometry(OSGeo.OGR.wkbGeometryType.wkbPolygon);
            poly.AddGeometry(ring);

            DataSource bnd_ocean_a = drv.CreateDataSource(_userFolder + mapName + @"\coast_area.shp", null);
            Layer lyr_bnd_ocean_a = bnd_ocean_a.CreateLayer("bnd_ocean_a", srs, wkbGeometryType.wkbPolygon, null);
            lyr_bnd_ocean_a.CreateField(fld_level, 0);

            DataSource osm_ds = Ogr.Open("/vsizip/" + vmapFolder, 0);
            if (osm_ds == null)
            {
                MessageBox.Show("Error processing Roads");
                return;
            }

            Layer layer = osm_ds.GetLayerByIndex(0);

            if (layer == null)
            {
                MessageBox.Show("Installation error. Please reinstall the software");
                return;
            }

            int n = osm_ds.GetLayerCount();


            layer = osm_ds.GetLayerByIndex(0);

            OSGeo.OGR.Feature f;
            layer.ResetReading();
            long cf = 0;
            long totF = layer.GetFeatureCount(0);

            while ((f = layer.GetNextFeature()) != null)
            {
                cf++;
                SetProgress((int)(cf * 100.0 / totF));

                var geom = f.GetGeometryRef();
                String name = f.GetFieldAsString(1);


                OSGeo.OGR.Feature nf = null;

                nf = new Feature(lyr_bnd_ocean_a.GetLayerDefn());
                //Envelope env = new Envelope();
                //geom.GetEnvelope(env) ;

                try
                {
                    nf.SetGeometry(geom.Intersection(poly));
                }
                catch
                {
                }
                nf.SetField(0, "");
                lyr_bnd_ocean_a.CreateFeature(nf);

                nf = null;
            }


            osm_ds.Dispose();
            osm_ds = null;

            bnd_ocean_a.Dispose();
            bnd_ocean_a = null;
        }


        private void processLakesVMAP(string vmapFolder, string mapName, double minLon, double minLat, double maxLon, double maxLat)
        {
            setStatus("Processing Lakes .. ");

            Ogr.RegisterAll();
            OSGeo.OGR.Driver drv = Ogr.GetDriverByName("ESRI Shapefile");
            OSGeo.OSR.SpatialReference srs = new OSGeo.OSR.SpatialReference(null);
            srs.ImportFromEPSG(4326);
            OSGeo.OGR.FieldDefn fld_level = new OSGeo.OGR.FieldDefn("LEVEL", FieldType.OFTString);


            OSGeo.OGR.Geometry ring = new OSGeo.OGR.Geometry(OSGeo.OGR.wkbGeometryType.wkbLinearRing);
            ring.AddPoint(minLon, minLat, 0);
            ring.AddPoint(minLon, maxLat, 0);
            ring.AddPoint(maxLon, maxLat, 0);
            ring.AddPoint(maxLon, minLat, 0);
            ring.AddPoint(minLon, minLat, 0);
            OSGeo.OGR.Geometry poly = new OSGeo.OGR.Geometry(OSGeo.OGR.wkbGeometryType.wkbPolygon);
            poly.AddGeometry(ring);

            DataSource water_area = drv.CreateDataSource(_userFolder + mapName + @"\water_area.shp", null);
            Layer lyr_water_area = water_area.CreateLayer("water_area", srs, wkbGeometryType.wkbPolygon, null);
            lyr_water_area.CreateField(fld_level, 0);

            DataSource osm_ds = Ogr.Open("/vsizip/" + vmapFolder, 0);
            if (osm_ds == null)
            {
                MessageBox.Show("Error processing Roads");
                return;
            }

            Layer layer = osm_ds.GetLayerByIndex(0);

            if (layer == null)
            {
                MessageBox.Show("Installation error. Please reinstall the software");
                return;
            }

            int n = osm_ds.GetLayerCount();


            layer = osm_ds.GetLayerByIndex(0);

            OSGeo.OGR.Feature f;
            layer.ResetReading();
            long cf = 0;
            long totF = layer.GetFeatureCount(0);

            while ((f = layer.GetNextFeature()) != null)
            {
                cf++;
                SetProgress((int)(cf * 100.0 / totF));

                var geom = f.GetGeometryRef();
                String name = f.GetFieldAsString(1);


                OSGeo.OGR.Feature nf = null;

                nf = new Feature(lyr_water_area.GetLayerDefn());
                //Envelope env = new Envelope();
                //geom.GetEnvelope(env) ;
 
                try
                {
                    nf.SetGeometry(geom. Intersection(poly));
                }
                catch
                {
                }
                nf.SetField(0, "");
                lyr_water_area.CreateFeature(nf);

                nf = null;
            }


            osm_ds.Dispose();
            osm_ds = null;

            water_area.Dispose();
            water_area = null;
        }

        private void downloadLakesVMAP(string vmapFile, string vmapFolder)
        {
            DataSource osm_ds = Ogr.Open("/vsizip/" + vmapFolder + "/" + vmapFile + ".zip/hydro-water-course-l.shp", 0);
            if (osm_ds != null)
            {
                osm_ds.Dispose();
                osm_ds = null;
                return;
            }
            string remoteUri = "http://osm/";
            DownloadFile(remoteUri, vmapFile + ".zip", vmapFolder);
        }

        private void processRiversVMAP(string vmapFolder, string mapName, double minLon, double minLat, double maxLon, double maxLat)
        {
            setStatus("Processing Rivers .. ");

            Ogr.RegisterAll();
            OSGeo.OGR.Driver drv = Ogr.GetDriverByName("ESRI Shapefile");
            OSGeo.OSR.SpatialReference srs = new OSGeo.OSR.SpatialReference(null);
            srs.ImportFromEPSG(4326);
            OSGeo.OGR.FieldDefn fld_level = new OSGeo.OGR.FieldDefn("LEVEL", FieldType.OFTString);


            OSGeo.OGR.Geometry ring = new OSGeo.OGR.Geometry(OSGeo.OGR.wkbGeometryType.wkbLinearRing);
            ring.AddPoint(minLon, minLat, 0);
            ring.AddPoint(minLon, maxLat, 0);
            ring.AddPoint(maxLon, maxLat, 0);
            ring.AddPoint(maxLon, minLat, 0);
            ring.AddPoint(minLon, minLat, 0);
            OSGeo.OGR.Geometry poly = new OSGeo.OGR.Geometry(OSGeo.OGR.wkbGeometryType.wkbPolygon);
            poly.AddGeometry(ring);

            DataSource water_line = drv.CreateDataSource(_userFolder + mapName + @"\water_line.shp", null);
            Layer lyr_water_line = water_line.CreateLayer("water_line", srs, wkbGeometryType.wkbLineString, null);
            lyr_water_line.CreateField(fld_level, 0);

            DataSource osm_ds = Ogr.Open("/vsizip/" + vmapFolder, 0);
            if (osm_ds == null)
            {
                MessageBox.Show("Error processing Roads");
                return;
            }

            Layer layer = osm_ds.GetLayerByIndex(0);

            if (layer == null)
            {
                MessageBox.Show("Installation error. Please reinstall the software");
                return;
            }

            int n = osm_ds.GetLayerCount();


            layer = osm_ds.GetLayerByIndex(0);

            OSGeo.OGR.Feature f;
            layer.ResetReading();
            long cf = 0;
            long totF = layer.GetFeatureCount(0);

            while ((f = layer.GetNextFeature()) != null)
            {
                cf++;
                SetProgress((int)(cf * 100.0 / totF));

                var geom = f.GetGeometryRef();
                String name = f.GetFieldAsString(1);


                OSGeo.OGR.Feature nf = null;

                nf = new Feature(lyr_water_line.GetLayerDefn());
                nf.SetGeometry(geom.Intersection(poly));
                nf.SetField(0, "");
                lyr_water_line.CreateFeature(nf);
  
                nf = null;
            }


            osm_ds.Dispose();
            osm_ds = null;

            water_line.Dispose();
            water_line = null;
        }



        private void downloadPlanetOSM(string osmFile, string osmFolder)
        {
            DataSource osm_ds = Ogr.Open("/vsizip/" + osmFolder + "/" + osmFile + ".zip/roads.shp", 0);
            if (osm_ds != null)
            {
                osm_ds.Dispose();
                osm_ds = null;
                return;
            }
            string remoteUri = "http://osm/"  ;
            DownloadFile(remoteUri, osmFile +  ".zip", _osmFolder);
        }

        private void processRoadsPlanetOSM(string osmFile, string mapName, double minLon, double minLat, double maxLon, double maxLat)
        {
            setStatus("Processing Roads .. ");

            Ogr.RegisterAll();
            OSGeo.OGR.Driver drv = Ogr.GetDriverByName("ESRI Shapefile");
            OSGeo.OSR.SpatialReference srs = new OSGeo.OSR.SpatialReference(null);
            srs.ImportFromEPSG(4326);
            OSGeo.OGR.FieldDefn fld_level = new OSGeo.OGR.FieldDefn("LEVEL", FieldType.OFTString);


            OSGeo.OGR.Geometry ring = new OSGeo.OGR.Geometry(OSGeo.OGR.wkbGeometryType.wkbLinearRing);
            ring.AddPoint(minLon, minLat, 0);
            ring.AddPoint(minLon, maxLat, 0);
            ring.AddPoint(maxLon, maxLat, 0);
            ring.AddPoint(maxLon, minLat, 0);
            ring.AddPoint(minLon, minLat, 0);
            OSGeo.OGR.Geometry poly = new OSGeo.OGR.Geometry(OSGeo.OGR.wkbGeometryType.wkbPolygon);
            poly.AddGeometry(ring);

            DataSource roadbig_line = drv.CreateDataSource(_userFolder + mapName + @"\roadbig_line.shp", null);
            Layer lyr_roadbig_line = roadbig_line.CreateLayer("roadbig_line", srs, wkbGeometryType.wkbLineString, null);
            lyr_roadbig_line.CreateField(fld_level, 0);

            DataSource roadmedium_line = drv.CreateDataSource(_userFolder + mapName + @"\roadmedium_line.shp", null);
            Layer lyr_roadmedium_line = roadbig_line.CreateLayer("roadmedium_line", srs, wkbGeometryType.wkbLineString, null);
            lyr_roadmedium_line.CreateField(fld_level, 0);

            DataSource roadsmall_line = drv.CreateDataSource(_userFolder + mapName + @"\roadsmall_line.shp", null);
            Layer lyr_roadsmall_line = roadsmall_line.CreateLayer("roadsmall_line", srs, wkbGeometryType.wkbLineString, null);
            lyr_roadsmall_line.CreateField(fld_level, 0);

            DataSource railroad_line = drv.CreateDataSource(_userFolder + mapName + @"\railroad_line.shp", null);
            Layer lyr_railroad_line = railroad_line.CreateLayer("roadsmall_line", srs, wkbGeometryType.wkbLineString, null);
            lyr_railroad_line.CreateField(fld_level, 0);

            
            DataSource osm_ds = Ogr.Open("/vsizip/" + osmFile , 0);
            if (osm_ds == null)
            {
                MessageBox.Show("Error processing Roads");
                return;
            }

            Layer layer = osm_ds.GetLayerByIndex(0);

            if (layer == null)
            {
                MessageBox.Show("Installation error. Please reinstall the software");
                return ;
            }

            int n = osm_ds.GetLayerCount();
           

            layer = osm_ds.GetLayerByIndex(0);

            OSGeo.OGR.Feature f;
            layer.ResetReading();
            long cf = 0;
            long totF = layer.GetFeatureCount(0);

            while ((f = layer.GetNextFeature()) != null)
            {
                cf++;
                SetProgress((int)(cf * 100.0 / totF));

                var geom = f.GetGeometryRef();
                String name = f.GetFieldAsString(1);


                OSGeo.OGR.Feature nf = null;
                if (name == "0" )
                {
                    nf = new Feature(lyr_roadbig_line.GetLayerDefn());
                    nf.SetGeometry(geom.Intersection(poly));
                    nf.SetField(0, "");
                    lyr_roadbig_line.CreateFeature(nf);
                }
                else  if (name == "1")
                {
                    nf = new Feature(lyr_roadmedium_line.GetLayerDefn());
                    nf.SetGeometry(geom.Intersection(poly));
                    nf.SetField(0, "");
                    lyr_roadmedium_line.CreateFeature(nf);
                }
                else if (name == "2")
                {
                    nf = new Feature(lyr_roadsmall_line.GetLayerDefn());
                    nf.SetGeometry(geom.Intersection(poly));
                    nf.SetField(0, "");
                    lyr_roadsmall_line.CreateFeature(nf);
                }
                else if (name == "3")
                {
                    nf = new Feature(lyr_railroad_line.GetLayerDefn());
                    nf.SetGeometry(geom.Intersection(poly));
                    nf.SetField(0, "");
                    lyr_railroad_line.CreateFeature(nf);
                }
                nf = null;
            }


            osm_ds.Dispose();
            osm_ds = null;

            roadbig_line.Dispose();
            roadbig_line = null;

            roadmedium_line.Dispose();
            roadmedium_line = null;

            roadsmall_line.Dispose();
            roadsmall_line = null;

            railroad_line.Dispose();
            railroad_line = null;
        }



        private void downloadVMAP(string vmapFile, string vmapFolder)
        {
            DataSource osm_ds = Ogr.Open("/vsizip/" + vmapFolder + "/" + vmapFile + ".zip/hydro-water-course-l.shp", 0);
            if (osm_ds != null)
            {
                osm_ds.Dispose();
                osm_ds = null;
                return;
            }
            string remoteUri = "http://osm/";
            DownloadFile(remoteUri, vmapFile + ".zip", vmapFolder);
        }



        private void processWaterOSM(string osmFile, string mapName)
        {
            setStatus("Processing Lakes ... ");

            Ogr.RegisterAll();
            OSGeo.OGR.Driver drv = Ogr.GetDriverByName("ESRI Shapefile");
            OSGeo.OSR.SpatialReference srs = new OSGeo.OSR.SpatialReference(null);
            srs.ImportFromEPSG(4326);


            OSGeo.OGR.FieldDefn fld_level = new OSGeo.OGR.FieldDefn("LEVEL", FieldType.OFTString);

            DataSource water_area = drv.CreateDataSource(_userFolder + mapName + @"\water_area.shp", null);
            Layer lyr_water_area = water_area.CreateLayer("water_area", srs, wkbGeometryType.wkbPolygon, null);
            lyr_water_area.CreateField(fld_level, 0);

            DataSource osm_ds = Ogr.Open(osmFile, 0);
            if (osm_ds == null)
            {
                MessageBox.Show("Error processing Rivers");
                return;
            }

            int n = osm_ds.GetLayerCount();
            //MessageBox.Show(n.ToString(CultureInfo.InvariantCulture));
            Layer layer = osm_ds.GetLayerByIndex(3);

            OSGeo.OGR.Feature f;
            layer.ResetReading();
            long cf = 0;
            long totF = layer.GetFeatureCount(0);
            while ((f = layer.GetNextFeature()) != null)
            {
                cf++;
                SetProgress((int)(cf * 100.0 / totF));

                var geom = f.GetGeometryRef();
                String id = f.GetFieldAsString(0);
                String name = f.GetFieldAsString(1);
                String highway = f.GetFieldAsString(2);

                OSGeo.OGR.Feature nf = null;

                nf = new Feature(lyr_water_area.GetLayerDefn());
                nf.SetGeometry(geom.SimplifyPreserveTopology(_simplify / (3600 * 30)));
                nf.SetField(0, name);
                lyr_water_area.CreateFeature(nf);


                nf = null;
            }


            osm_ds.Dispose();
            osm_ds = null;

            water_area.Dispose();
            water_area = null;
        }

        private void processRiverOSM(string osmFile, string mapName)
        {
            setStatus("Processing Rivers ... ");

            Ogr.RegisterAll();
            OSGeo.OGR.Driver drv = Ogr.GetDriverByName("ESRI Shapefile");
            OSGeo.OSR.SpatialReference srs = new OSGeo.OSR.SpatialReference(null);
            srs.ImportFromEPSG(4326);


            OSGeo.OGR.FieldDefn fld_level = new OSGeo.OGR.FieldDefn("LEVEL", FieldType.OFTString);

            DataSource water_line = drv.CreateDataSource(_userFolder + mapName + @"\water_line.shp", null);
            Layer lyr_water_line = water_line.CreateLayer("water_line", srs, wkbGeometryType.wkbLineString, null);
            lyr_water_line.CreateField(fld_level, 0);

            DataSource osm_ds = Ogr.Open(osmFile, 0);
            if (osm_ds == null)
            {
                MessageBox.Show("Error processing Rivers");
                return;
            }

            int n = osm_ds.GetLayerCount();
            //MessageBox.Show(n.ToString(CultureInfo.InvariantCulture));
            Layer layer = osm_ds.GetLayerByIndex(1);

            OSGeo.OGR.Feature f;
            layer.ResetReading();
            long cf = 0;
            long totF = layer.GetFeatureCount(0);
            while ((f = layer.GetNextFeature()) != null)
            {
                SetProgress((int)(cf * 100.0 / totF));

                var geom = f.GetGeometryRef();
                String id = f.GetFieldAsString(0);
                String name = f.GetFieldAsString(1);
                String highway = f.GetFieldAsString(2);

                OSGeo.OGR.Feature nf = null;

                nf = new Feature(lyr_water_line.GetLayerDefn());
                nf.SetGeometry(geom.SimplifyPreserveTopology(_simplify / (3600 * 30)));
                nf.SetField(0, "0");
                lyr_water_line.CreateFeature(nf);


                nf = null;
            }


            osm_ds.Dispose();
            osm_ds = null;

            water_line.Dispose();
            water_line = null;
        }

        private void processRailwayOSM(string osmFile, string mapName)
        {
            setStatus("Processing Railway ... ");

            Ogr.RegisterAll();
            OSGeo.OGR.Driver drv = Ogr.GetDriverByName("ESRI Shapefile");
            OSGeo.OSR.SpatialReference srs = new OSGeo.OSR.SpatialReference(null);
            srs.ImportFromEPSG(4326);


            OSGeo.OGR.FieldDefn fld_level = new OSGeo.OGR.FieldDefn("LEVEL", FieldType.OFTString);

            DataSource railroad_line = drv.CreateDataSource(_userFolder + mapName + @"\railroad_line.shp", null);
            Layer lyr_railroad_line = railroad_line.CreateLayer("railroad_line", srs, wkbGeometryType.wkbLineString, null);
            lyr_railroad_line.CreateField(fld_level, 0);

            DataSource osm_ds = Ogr.Open(osmFile, 0);
            if (osm_ds == null)
            {
                MessageBox.Show("Error processing Railway");
                return;
            }

            int n = osm_ds.GetLayerCount();
            //MessageBox.Show(n.ToString(CultureInfo.InvariantCulture));
            Layer layer = osm_ds.GetLayerByIndex(1);

            OSGeo.OGR.Feature f;
            layer.ResetReading();
            long cf = 0;
            long totF = layer.GetFeatureCount(0);
            while ((f = layer.GetNextFeature()) != null)
            {
                SetProgress((int)(cf * 100.0 / totF));

                var geom = f.GetGeometryRef();
                String id = f.GetFieldAsString(0);
                String name = f.GetFieldAsString(1);
                String highway = f.GetFieldAsString(2);

                OSGeo.OGR.Feature nf = null;
                             
                nf = new Feature(lyr_railroad_line.GetLayerDefn());
                nf.SetGeometry(geom.SimplifyPreserveTopology(_simplify / (3600 * 30)));
                nf.SetField(0, "0");
                lyr_railroad_line.CreateFeature(nf);
                

                nf = null;
            }


            osm_ds.Dispose();
            osm_ds = null;

            railroad_line.Dispose();
            railroad_line = null;

        }

        private void processRoadsOSM(string osmFile, string mapName)
        {

            setStatus("Processing roads ... ");

            Ogr.RegisterAll();
            OSGeo.OGR.Driver drv = Ogr.GetDriverByName("ESRI Shapefile");
            OSGeo.OSR.SpatialReference srs = new OSGeo.OSR.SpatialReference(null);
            srs.ImportFromEPSG(4326);


            OSGeo.OGR.FieldDefn fld_level = new OSGeo.OGR.FieldDefn("LEVEL", FieldType.OFTString);
            
            DataSource roadbig_line = drv.CreateDataSource(_userFolder + mapName + @"\roadbig_line.shp", null);
            Layer lyr_roadbig_line = roadbig_line.CreateLayer("roadbig_line",  srs,  wkbGeometryType.wkbLineString ,null);
            lyr_roadbig_line.CreateField(fld_level,0);

            DataSource roadmedium_line = drv.CreateDataSource(_userFolder + mapName + @"\roadmedium_line.shp", null);
            Layer lyr_roadmedium_line = roadbig_line.CreateLayer("roadmedium_line", srs, wkbGeometryType.wkbLineString, null);
            lyr_roadmedium_line.CreateField(fld_level, 0);

            DataSource roadsmall_line = drv.CreateDataSource(_userFolder + mapName + @"\roadsmall_line.shp", null);
            Layer lyr_roadsmall_line = roadsmall_line.CreateLayer("roadsmall_line", srs, wkbGeometryType.wkbLineString, null);
            lyr_roadsmall_line.CreateField(fld_level, 0);



            DataSource osm_ds = Ogr.Open(osmFile, 0);
            if (osm_ds == null)
            {
                MessageBox.Show("Error processing Roads");
                return;
            }

            int n = osm_ds.GetLayerCount();
            //MessageBox.Show(n.ToString(CultureInfo.InvariantCulture));
            Layer layer = osm_ds.GetLayerByIndex(1);

            OSGeo.OGR.Feature f;
            layer.ResetReading();
            long cf = 0;
            long totF = layer.GetFeatureCount(0);
            double _simplify = 100;
            while ((f = layer.GetNextFeature()) != null)
            {
                SetProgress((int)(cf * 100.0 / totF));

                var geom = f.GetGeometryRef();
                String id = f.GetFieldAsString(0) ;
                String name = f.GetFieldAsString(1);
                String highway = f.GetFieldAsString(2);

                OSGeo.OGR.Feature nf = null;
                if (highway == "motorway" || highway == "trunk")
                {
                    nf = new Feature(lyr_roadbig_line.GetLayerDefn());
                    nf.SetGeometry(geom.SimplifyPreserveTopology(_simplify / (3600 * 30)));
                    nf.SetField(0, "0");
                    lyr_roadbig_line.CreateFeature(nf);
                }
                else if (highway == "primary")
                {
                    nf = new Feature(lyr_roadmedium_line.GetLayerDefn());
                    nf.SetGeometry(geom.SimplifyPreserveTopology(_simplify / (3600 * 30)));
                    nf.SetField(0, "0");
                    lyr_roadmedium_line.CreateFeature(nf);
                }
                else if (highway == "secondary" || highway == "tertiary")
                {
                    nf = new Feature(lyr_roadsmall_line.GetLayerDefn());
                    nf.SetGeometry(geom.SimplifyPreserveTopology(_simplify / (3600 * 30)));
                    nf.SetField(0, "0");
                    lyr_roadsmall_line.CreateFeature(nf);
                }
                nf = null;
            }


            osm_ds.Dispose();
            osm_ds = null;

            roadbig_line.Dispose();
            roadbig_line = null;


            roadmedium_line.Dispose();
            roadmedium_line = null;


            roadsmall_line.Dispose();
            roadsmall_line = null;

        }



        private void downloadOSM(string api_url, string filename,string destFolder)
        {

            setStatus("Querying OpenStreetMap data ... wait ..");



            var wreq = WebRequest.Create(api_url);
            wreq.Timeout = Timeout.Infinite;

            var wresp = (HttpWebResponse)wreq.GetResponse();

            using (Stream file = File.OpenWrite(destFolder + filename))
            {
                wresp.GetResponseStream().CopyTo(file); // CopyTo extension only .net 4.0+ 
            }


            return;

            downloadComplete = false;

            using (WebClient webClient = new WebClient())
            {

                //webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(Completed);
                //webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);


                Uri URL = new Uri(api_url);

                try
                {
                    // Start downloading the file
                    webClient.DownloadFile(URL, destFolder + filename);
                    //while (!downloadComplete)
                    //{
                    //    Application.DoEvents();
                    //}
                    //downloadComplete = false;
                    
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                
            }
            
        }



        private void backgroundWorkerLKM_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

        }

        private void backgroundWorkerLKM_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

        }



        private void trackBarPixelSize_ValueChanged_1(object sender, EventArgs e)
        {
            _cellSize = 10 *  (int)(trackBarPixelSize.Value / 1000);
            labelPixelSize.Text = _cellSize.ToString(CultureInfo.InvariantCulture);
        }

        private void buttonOptions_Click(object sender, EventArgs e)
        {
            FormOption frm = new FormOption(this);
            frm.ShowDialog();
        }

        private void buttonOfflineTopology_Click(object sender, EventArgs e)
        {
            _cellSize = Convert.ToInt32(labelPixelSize.Text);
            _outFolder = textBoxOutFolder.Text;
            _nDownloaded = 0;
            _mapName = textBoxMapName.Text;

            if (!Directory.Exists(_userFolder + "/" + _mapName))
            {
                Directory.CreateDirectory(_userFolder + "/" + _mapName);
            }

            if (MessageBox.Show("For offline processing you need to download a osm.pbf file for your area from internet\nDo you want to be redirect to the download page?", "Offline", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
            {
                System.Diagnostics.Process.Start("http://download.geofabrik.de/index.html");
                return;
            }

            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "osm xml (*.osm)|*.osm|osm.pbf (*.pbf)|*.pbf";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            if(openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                _offlineOSMFile = openFileDialog1.FileName; 
                backgroundWorkerOffline.RunWorkerAsync();
            }

            
        }

        private void backgroundWorkerOffline_DoWork(object sender, DoWorkEventArgs e)
        {

            startFakeProgress();

            enableControls(false);

            _step = 1; // Download OSM
            _nTiles = 1;
            NSTEPS = 8;


            _step++;
            processOSM(_offlineOSMFile, _mapName, _minLon, _minLat, _maxLon, _maxLat);
            _step++;

            fixDBF_LDID(_mapName); 

            stopsFakeProgress();

            _step++;
            processOcean(_mapName, _minLon, _minLat, _maxLon, _maxLat);

            _step++;
            createTBLFile(_mapName);

            _step++;
            //createFinalZip(_mapName, _outFolder);
            createFinalZipNew(_mapName, _outFolder);

            _step = NSTEPS;


            enableControls(true);

            _step = NSTEPS;
            SetProgress(100);

            MessageBox.Show("Done");
        }

        private void fixDBF_LDID(string mapName)
        {

            string mapfolder = _userFolder + mapName;
            string[] files = Directory.GetFiles(mapfolder, "*.dbf");
            foreach  (string fn in files)
            {
                byte[] bytes = File.ReadAllBytes(fn);
                if ( bytes.Length > 29  )
                {
                    bytes[29] = 0x00;
                    File.WriteAllBytes(fn, bytes);
                }
            }




        }






        /*       private char normalizeChar( char c)
               {
                   switch ( c)
                   {
                       case 'ą':
                           return 'a';
                       case 'ć':
                           return 'c';
                       case 'ę':
                           return 'e';
                       case 'ł':
                           return 'l';
                       case 'ń':
                           return 'n';
                       case 'ó':
                           return 'o';
                       case 'ś':
                           return 's';
                       case 'ż':
                       case 'ź':
                           return 'z';
                   }
                   return c;
               }


               static string RemoveDiacritics(string name)
               {


                   string asciiEquivalents = Encoding.ASCII.GetString(
                                   Encoding.GetEncoding("Cyrillic").GetBytes(name)
                               );

                   return asciiEquivalents;


               }*/

    }



}
