  private void RunScript(Line line, Plane plane, Surface sphere, Circle circle, ref object A, ref object B, ref object C)
  {

    Component.Message = "Geometry Intersections";

    //Line Sphere
    Point3d[] pts = new Point3d[2];
    Sphere s;
    sphere.TryGetSphere(out s);
    Rhino.Geometry.Intersect.Intersection.LineSphere(line, s, out pts[0], out pts[1]);

    //Plane Sphere
    Circle c;
    Rhino.Geometry.Intersect.Intersection.PlaneSphere(plane, s, out c);

    //Curve Curve
    Rhino.Geometry.Intersect.CurveIntersections ie = Rhino.Geometry.Intersect.Intersection.CurveCurve(line.ToNurbsCurve(), circle.ToNurbsCurve(), Rhino.RhinoDoc.ActiveDoc.ModelAbsoluteTolerance, 0.05);
    List<Point3d> points = new List<Point3d>();

    //Collect start points
    for(int i = 0; i < ie.Count; i++){
      points.Add(ie[i].PointA);
    }

    A = pts;
    B = c;
    C = points;