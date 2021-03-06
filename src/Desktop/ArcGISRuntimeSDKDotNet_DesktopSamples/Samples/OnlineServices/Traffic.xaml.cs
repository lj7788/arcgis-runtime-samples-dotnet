﻿using Esri.ArcGISRuntime.Controls;
using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Layers;
using Esri.ArcGISRuntime.Security;
using Esri.ArcGISRuntime.Tasks.Query;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ArcGISRuntimeSDKDotNet_DesktopSamples.Samples
{
    /// <summary>
    /// This sample shows how to add the ArcGIS Traffic service to a map.
    /// </summary>
    /// <title>Traffic</title>
    /// <category>ArcGIS Online Services</category>
    public partial class Traffic : UserControl
    {
        private ArcGISDynamicMapServiceLayer _trafficLayer;

        public Traffic()
        {
            InitializeComponent();

            IdentityManager.Current.ChallengeMethod = PortalSecurity.Challenge;

            _trafficLayer = mapView.Map.Layers["Traffic"] as ArcGISDynamicMapServiceLayer;

            mapView.Map.InitialExtent = new Envelope(-13230693.582, 3941779.273, -12928937.030, 4095486.517, SpatialReferences.WebMercator);
            mapView.LayerLoaded += mapView_LayerLoaded;
        }

        // Populate layer legend with north america traffic sublayer names
        private async void mapView_LayerLoaded(object sender, LayerLoadedEventArgs e)
        {
            if (e.Layer == _trafficLayer)
            {
                var legendLayer = _trafficLayer as ILegendSupport;
                var layerLegendInfo = await legendLayer.GetLegendInfosAsync();
                legendTree.ItemsSource = layerLegendInfo.LayerLegendInfos.First().LayerLegendInfos;
            }
        }

        private async void mapView_MapViewTapped(object sender, MapViewInputEventArgs e)
        {
            try
            {
                incidentOverlay.Visibility = Visibility.Collapsed;
                incidentOverlay.DataContext = null;

                var identifyTask = new IdentifyTask(new Uri(_trafficLayer.ServiceUri));

                IdentifyParameter identifyParams = new IdentifyParameter(e.Location, mapView.Extent, 5, (int)mapView.ActualHeight, (int)mapView.ActualWidth)
                {
                    LayerIDs = new int[] { 2, 3, 4 },
                    LayerOption = LayerOption.Top,
                    SpatialReference = mapView.SpatialReference,
                };

                var result = await identifyTask.ExecuteAsync(identifyParams);

                if (result != null && result.Results != null && result.Results.Count > 0)
                {
                    incidentOverlay.DataContext = result.Results.First();
                    incidentOverlay.Visibility = Visibility.Visible;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Identify Error");
            }
        }
    }
}
