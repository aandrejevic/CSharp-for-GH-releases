  private void RunScript(int n1, int n2, int n3, double dist, ref object A)
  {

    Component.Message = "PointCloud Display";


    PointCloud pointCloud = new PointCloud();

    for (int i = 0; i < n1 ; i++)
      for(int j = 0; j < n2; j++)
        for(int k = 0; k < n3; k++)
          pointCloud.Add(new Point3d(i * dist, j * dist, k * dist), Color.FromArgb(i * 2 % 255, j * 2 % 255, k * 2 % 255));

    _pointCloud = pointCloud;


  }

  // <Custom additional code> 
  PointCloud _pointCloud = new PointCloud();

  public override BoundingBox ClippingBox
  {
    get { return _pointCloud.GetBoundingBox(false);}
  }

  public override void DrawViewportMeshes(IGH_PreviewArgs args){
    args.Display.DrawPointCloud(_pointCloud, 5);
  }

