﻿using Esri.ArcGISRuntime.Geometry;
using Windows.UI.Xaml.Controls;


namespace ArcGISRuntimeSDKDotNet_StoreSamples.Samples
{

	/// <summary>
	/// 
	/// </summary>
	/// <category>Mapping</category>
	public sealed partial class DefineMapProjection : Page
    {
        public DefineMapProjection()
        {
            this.InitializeComponent();
            mapView1.Map.InitialExtent = new Envelope(661140, -1420246, 3015668, 1594451) { SpatialReference = new SpatialReference(26777) };

        }


    }
}
