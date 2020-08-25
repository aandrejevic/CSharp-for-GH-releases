private void RunScript(int nX, int nY, ref object A, ref object B, ref object C)
  {


    List<Point3d> pts = new List<Point3d>();

    for (int i = 0; i < 30; i++)
      for(int j = 0; j < 30; j++)
        pts.Add(new Point3d(i, j, (Math.Cos(i) * Math.Sin(j))));

    Rhino.Geometry.NurbsSurface surf = Rhino.Geometry.NurbsSurface.CreateFromPoints(pts, 30, 30, 2, 2);


    double width;
    double height;

    List<Rhino.Geometry.Surface> surfaces = new List<Surface>();
    surf.GetSurfaceSize(out width, out height);


    for (int i = 0; i < nX; i++){
      for (int j = 0; j < nY; j++){

        double xStart = width / nX * i;
        double xEnd = xStart + width / nX;

        double yStart = height / nY * j;
        double yEnd = yStart + height / nY;

        Surface trimSurf = surf.Trim(new Interval(xStart, xEnd), new Interval(yStart, yEnd));
        surfaces.Add(trimSurf);

      }
    }

    var  count = surfaces.Count();

    A = surf;
    B = count;
    C = surfaces;
  }
