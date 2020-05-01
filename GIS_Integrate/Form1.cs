using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AxMapWinGIS;
using MapWinGIS;

namespace GIS_Integrate
{
    public partial class Form1 : Form
    {
        public static Dictionary<string, int> layerControl = new Dictionary<string, int>();
        public Form1()
        {
            InitializeComponent();
            //axMap1.Projection = tkMapProjection.PROJECTION_GOOGLE_MERCATOR;
            axMap1.Projection = tkMapProjection.PROJECTION_WGS84;
            axMap1.KnownExtents = tkKnownExtents.keUSA;
            axMap1.CursorMode = tkCursorMode.cmPan;
        }

        private void btnExplorer_Click(object sender, EventArgs e)
        {
            
        }

        private void btnExplorer_Click_1(object sender, EventArgs e)
        {
            //string filename = @"C:\Work\GIS\data\EB Greenwood_profile_shapefile_csv.shp";
            string filename = @"D:\shp\county.shp";
            //string filename = @"C:\Work\GIS\data\Pangaea.png";
            //string filename = @"C:\Work\GIS\data\StateFair_LangWithChuk_2018.jpg";
            var fm = new FileManager();
            if (!fm.get_IsSupported(filename))
            {
                MessageBox.Show("Data source isn't supported by MapWinGIS");
            }
            else
            {
                var obj = fm.Open(filename, tkFileOpenStrategy.fosAutoDetect, null);
                if (fm.LastOpenIsSuccess)
                {
                    if (fm.LastOpenStrategy == tkFileOpenStrategy.fosVectorLayer)
                    {
                        var shapefile = obj as Shapefile;
                        if (shapefile != null)
                        {
                            //MessageBox.Show("Shapefile was opened.");
                            int handle = axMap1.AddLayer(obj, true);
                            axMap1.ZoomToLayer(handle);
                            //ZoomToValue(shapefile, "State_Name", "Iowa");

                            if (handle != -1)
                            {
                                //MessageBox.Show("Layer was added to the map. Open strategy: " + fm.LastOpenStrategy.ToString());
                                string fname = filename.Split('\\').Last();
                                fname = fname.Remove(fname.Length - 4);  //get rid of the .shp
                                treeView1.CheckBoxes = true;
                                treeView1.Nodes.Add(fname);
                                //treeView1.Nodes[1].Nodes[0].Nodes[0].Checked = true;
                                if (layerControl.Count > 0)
                                {
                                    if (layerControl[fname] < 0)
                                        layerControl.Add(fname, handle);
                                    else layerControl.Add(fname + handle, handle);
                                }
                                else layerControl.Add(fname, handle);
                            }
                            else
                            {
                                MessageBox.Show("Failed to add layer to the map: " + axMap1.get_ErrorMsg(axMap1.LastErrorCode));
                            }
                        }
                    }
                    else
                    {
                        var image = obj as MapWinGIS.Image;
                        if (image != null)
                            MessageBox.Show("Image was opened.");
                        int handle = axMap1.AddLayer(obj, true);
                        axMap1.ZoomToLayer(handle);
                        if (handle != -1)
                        {
                            MessageBox.Show("Layer was added to the map. Open strategy: " + fm.LastOpenStrategy.ToString());
                        }
                        else
                        {
                            MessageBox.Show("Failed to add layer to the map: " + axMap1.get_ErrorMsg(axMap1.LastErrorCode));
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Failed to open datasource: " + fm.get_ErrorMsg(fm.LastErrorCode));
                }
            }
        }
    }
}
