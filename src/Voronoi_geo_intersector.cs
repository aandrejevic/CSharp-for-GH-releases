  private void RunScript(Box box, List<Brep> bList, object void0, List<double> vC, object void1, double x, double y, double D, ref object brep, ref object diffBrepList)
  {

    Vector3d dir = new Vector3d(vC[0], vC[1], vC[2]);
    Plane plane = new Plane (box.Center, dir);

    Interval xi = new Interval(-x, x);
    Interval yi = new Interval(-y, y);

    Rectangle3d rect = new Rectangle3d(plane, xi, yi);

    Brep b = Brep.CreatePlanarBreps(rect.ToNurbsCurve())[0];
    BrepFace bf = b.Faces[0];

    Brep extr = Brep.CreateFromOffsetFace(bf, D, 0.01, true, true);
    brep = extr;

    List<Brep> sdiff = new List<Brep>();
    foreach(Brep br in bList){
      try{
        Brep diff = Brep.CreateBooleanIntersection(extr, br, 0.01)[0];
        sdiff.Add(diff);
      }
      catch{}
    }
    diffBrepList = sdiff;