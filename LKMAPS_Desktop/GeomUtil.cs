// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright The LKMap Desktop Project

using System;
using System.Collections.Generic;
using OSGeo.OGR;
using System.Windows.Forms;
using System.IO;

namespace LKMAPS_Desktop
{
    public static class GDALUtil
    {
        public static List<String> getSRTM30Tiles(double latmin, double lonmin, double latmax, double lonmax)
        {
            List<String> ret = new List<String>();
            string appPath = Path.GetDirectoryName(Application.ExecutablePath);

            //Ogr.RegisterAll();
            OSGeo.OGR.Ogr.RegisterAll();
            DataSource ds1 = Ogr.Open("/vsizip/" + appPath + "/data.zip/index/srtm30.shp", 0);

            if (ds1 == null)
            {
                MessageBox.Show("Installation error. Please reinstall the software");
                return ret;
            }

            Layer layer = ds1.GetLayerByIndex(0);


            //Layer layer = ds1.GetLayerByName("features");

            

            if (layer == null)
            {
                MessageBox.Show("Installation error. Please reinstall the software");
                return ret;
            }

            OSGeo.OGR.Feature f;
            layer.ResetReading();

            OSGeo.OGR.Geometry area = new OSGeo.OGR.Geometry(OSGeo.OGR.wkbGeometryType.wkbPolygon);
            OSGeo.OGR.Geometry ring = new OSGeo.OGR.Geometry(OSGeo.OGR.wkbGeometryType.wkbLinearRing);
            ring.AddPoint(lonmin, latmin, 0);
            ring.AddPoint(lonmin, latmax, 0);
            ring.AddPoint(lonmax, latmax, 0);
            ring.AddPoint(lonmax, latmin, 0);
            ring.AddPoint(lonmin, latmin, 0);
            area.AddGeometry(ring);


            while ((f = layer.GetNextFeature()) != null)
            {
                var geom = f.GetGeometryRef();
                if (geom != null)
                {
                    if (geom.Intersect(area))
                    {
                        String tile = f.GetFieldAsString(2);
                        ret.Add(tile);
                    }
                }
            }

            return ret;
        }




        public static List<String> getSRTM90Tiles(double latmin, double lonmin, double latmax, double lonmax)
        {
             List<String> ret = new  List<String>();
             string appPath = Path.GetDirectoryName(Application.ExecutablePath);

            //Ogr.RegisterAll();
            OSGeo.OGR.Ogr.RegisterAll();
            DataSource ds1;
            if (true)
            {
                ds1 = Ogr.Open("/vsizip/" + appPath + "/data.zip/index/viewfinderpanoramas.shp", 0);
            }
            else
            {
                ds1 = Ogr.Open("/vsizip/" + appPath + "/data.zip/index/srtm90.shp", 0);

            }

            if (ds1 == null)
            {
                MessageBox.Show("Installation error. Please reinstall the software");
                return ret;
            }

            Layer layer = ds1.GetLayerByIndex(0);

            if (layer == null)
            {
                MessageBox.Show("Installation error. Please reinstall the software");
                return ret;
            }

            OSGeo.OGR.Feature f;
            layer.ResetReading();

            OSGeo.OGR.Geometry ring = new OSGeo.OGR.Geometry(OSGeo.OGR.wkbGeometryType.wkbLinearRing);
            ring.AddPoint(lonmin,latmin,0);
            ring.AddPoint(lonmin,latmax,0);
            ring.AddPoint(lonmax,latmax,0);
            ring.AddPoint(lonmax,latmin,0);
            ring.AddPoint(lonmin,latmin,0);
            OSGeo.OGR.Geometry poly = new OSGeo.OGR.Geometry(OSGeo.OGR.wkbGeometryType.wkbPolygon);
            poly.AddGeometry(ring);
             

            while ((f = layer.GetNextFeature()) != null)
            {
                var geom = f.GetGeometryRef();
                if (geom != null)
                {
                    if (geom.Intersect(poly) || poly.Contains(geom))
                    {
                        String tile = f.GetFieldAsString(2);
                        ret.Add(tile);
                    }
                }
            }

            return ret;
        }


        internal static string GetVMAPArea(double minLon, double minLat, double maxLon, double maxLat)
        {
            double lat = (maxLat + minLat) / 2;
            double lon = (minLon + maxLon) / 2;
            string appPath = Path.GetDirectoryName(Application.ExecutablePath);

            //Ogr.RegisterAll();
            OSGeo.OGR.Ogr.RegisterAll();
            DataSource ds1 = Ogr.Open("/vsizip/" + appPath + "/data.zip/index/vmap0.shp", 0);

            if (ds1 == null)
            {
                MessageBox.Show("Installation error. Please reinstall the software");
                return "";
            }

            Layer layer = ds1.GetLayerByIndex(0);

            if (layer == null)
            {
                MessageBox.Show("Installation error. Please reinstall the software");
                return "";
            }

            OSGeo.OGR.Feature f;
            layer.ResetReading();

            OSGeo.OGR.Geometry p = new OSGeo.OGR.Geometry(OSGeo.OGR.wkbGeometryType.wkbPoint);
            p.AddPoint(lon, lat, 0);



            while ((f = layer.GetNextFeature()) != null)
            {
                var geom = f.GetGeometryRef();
                if (geom != null)
                {
                    if (   geom.Contains(p) )
                    {
                        return f.GetFieldAsString(1);
                        
                    }
                }
            }

            return "";
        }
    }
}
