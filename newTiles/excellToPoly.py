# -*- coding: utf-8 -*-  
#-------------------------------------------------------------------------------  
# Name:        PuntosPoligonosTXT.py  
# Purpose:    covertir puntos a poligonos  
#  
# Author:      xbakker  
#  
# Created:    10/04/2015  
#-------------------------------------------------------------------------------  

import arcpy  
import os  
  
def main():  
    import sys  
    import traceback  
  
    try:  
        
        arcpy.AddMessage("Running ... ")  

        arcpy.env.overwriteOutput = True  
  
        # first parameter is txt file  
        txt = arcpy.GetParameterAsText(0)  
  
        # forth parameter (provide default NOMBRE)  
         
  
        # second parameter is output fc  
        fc_out = arcpy.GetParameterAsText(2)  
  
        # third parameter is spatial reference  
        sr = arcpy.GetParameter(1)  
  
        # create empty output fc  
        ws_name, fc_name = os.path.split(fc_out)  
        geomtype = "POLYGON"  
        arcpy.CreateFeatureclass_management(ws_name, fc_name, geomtype, spatial_reference=sr)  
  
        # add field  
        arcpy.AddField_management(fc_out, "URL", "TEXT", field_length=512)  
        arcpy.AddField_management(fc_out, "Tile", "TEXT", field_length=30)  
        arcpy.AddField_management(fc_out, "File", "TEXT", field_length=512)  
        arcpy.AddField_management(fc_out, "LonMin", "DOUBLE")  
        arcpy.AddField_management(fc_out, "LonMax", "DOUBLE")  
        arcpy.AddField_management(fc_out, "LatMin", "DOUBLE")  
        arcpy.AddField_management(fc_out, "LatMax", "DOUBLE")  

  
        # start insert cursor  
        flds = ("SHAPE@", "URL","Tile","File","LonMin","LonMax","LatMin","LatMax")  
        with arcpy.da.InsertCursor(fc_out, flds) as curs:  
  
            # read input file  
            cnt = 0  
            nombre = ""  
            first_point = None  
            lst_pnt = []  
  
            with open(txt, 'r') as f:  
                i = 0
                for line in f.readlines():  
                    i = i + 1
                    if ( i == 1 ):
                        continue
                    i = i + 1
                    line = line.replace('\n','')  
                    lst_line = line.split('\t')  
                    
                    URL = lst_line[4]
                    file = lst_line[5] 
                    Tile = lst_line[6]
                    XMAX = float(lst_line[9])  
                    XMIN = float(lst_line[7])  
                    YMAX = float(lst_line[10])  
                    YMIN = float(lst_line[8])  
                    pnt1 = arcpy.Point(XMIN, YMIN)  
                    pnt2 = arcpy.Point(XMIN, YMAX)  
                    pnt3 = arcpy.Point(XMAX, YMAX)  
                    pnt4 = arcpy.Point(XMAX, YMIN)  
                    array = arcpy.Array()  
                    array.add(pnt1)  
                    array.add(pnt2)  
                    array.add(pnt3)  
                    array.add(pnt4)  
                    array.add(pnt1)  
                    polygon = arcpy.Polygon(array)  
                    curs.insertRow((polygon, URL,file,Tile,XMIN,XMAX,YMIN,YMAX ))    

                    arcpy.AddMessage("Tile: '{0}'".format(Tile))  
  

        arcpy.AddMessage("Done")  

    except:  
        tb = sys.exc_info()[2]  
        tbinfo = traceback.format_tb(tb)[0]  
        pymsg = "Errores de Python:\nTraceback info:\n" + tbinfo + "\nError Info:\n" + str(sys.exc_info()[1])  
        msgs = "Errores de ArcPy:\n" + arcpy.GetMessages(2) + "\n"  
        arcpy.AddError(pymsg)  
        arcpy.AddError(msgs)  
  
  

  
if __name__ == '__main__':  
    main()  