private void RunScript(int n, double dist, ref object A)
  {

    Component.Message = "Create DataTree";

    DataTree<Point3d> points = new DataTree<Point3d>();

    for (int i = 0; i < n ; i++)
      for(int j = 0; j < n; j++)
        for(int k = 0; k < n; k++)
          points.Add(new Point3d(i * dist, j * dist, k * dist), new GH_Path(i, j));

    A = points;



  }