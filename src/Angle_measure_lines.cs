private void RunScript(Polyline poly , double currPtR, double prevPtR, double nextPtR, int c, ref object planeP, ref object currentP, ref object prevP, ref object nextP)
  {


    if(!poly.IsClosed) {poly.Add(poly[0]);Print("Note: Polyline found open (closed)");}
    if(poly.Count < 5){Print("Note: this is a triangle: Adios");return;}

    Print("count: {0}", poly.Count);


    if(!ClockwisePoly(poly)){Print("Note: Polyline reversed (to clockwise)");poly.Reverse();}

    if(c > poly.Count - 2) c = poly.Count - 2;

    Point3d prev;
    try{ prev = poly[c - 1];} catch{ prev = poly[poly.Count - 2];}
    Point3d current = poly[c];
    Point3d next = poly[c + 1];

    Plane plane;
    double vAngle = MeasureAngle(prev, current, next, out plane);
    Print("For corner: {0}, angle: {1}", c, Math.Round(RhinoMath.ToDegrees(vAngle), 2));

    Circle currCirc = new Circle(plane, currPtR);
    currentP = Brep.CreatePlanarBreps(currCirc.ToNurbsCurve());

    Sphere prevSphere = new Sphere(prev, prevPtR); prevP = prevSphere;
    Sphere nextSphere = new Sphere(next, nextPtR);nextP = nextSphere;

    planeP = plane;

  }

  // <Custom additional code> 

  private bool ClockwisePoly(Polyline poly){

    double[,] Matrix = new double[poly.Count + 1, 3];

    List<double> rrIz = new List<double>();
    List<double> rrDe = new List<double>();

    double sumaIz = 0; double sumaDe = 0; double area = 0;

    for (int i = 0; i < poly.Count; i++) {
      Matrix[i, 0] = poly[i].X;
      Matrix[i, 1] = poly[i].Y;
      Matrix[i, 2] = poly[i].Z;
    }
    for (int i = 0; i < poly.Count - 1; i++) {
      rrIz.Add(Matrix[i, 0] * Matrix[i + 1, 1]);
      rrDe.Add(Matrix[i + 1, 0] * Matrix[i, 1]);
    }
    for (int i = 0; i <= rrIz.Count - 1; i++) {
      sumaIz += rrIz[i];
      sumaDe += rrDe[i];
    }
    area = 0.5 * ((sumaIz) -(sumaDe));
    if (area > 0) {
      return false;
    } else {
      return true;
    }
  }

  private double MeasureAngle(Point3d prev, Point3d current, Point3d next, out Plane planeOUT){
    //compute angle
    Plane plane = new Plane (current, prev, next); //planeP = plane;
    Vector3d vPrev = prev - current; vPrev.Unitize();
    Vector3d vNext = next - current; vNext.Unitize();
    double vAngle = Vector3d.VectorAngle(vPrev, vNext, plane);
    //check angle
    Vector3d nextToCurr = current - next;
    Vector3d currToPrev = prev - current;
    Vector3d cross = CrossProduct(nextToCurr, currToPrev);
    if(cross.Z < 0)  vAngle = Math.PI * 2 - vAngle;

    planeOUT = plane;
    return vAngle;
  }

  public double DotProduct(Vector3d v1, Vector3d v2){
    double dot = (v1.X * v2.X) + (v1.Y * v2.Y) + (v1.Z * v2.Z);
    return dot;
  }

  public Vector3d CrossProduct(Vector3d V1, Vector3d V2){
    Vector3d cross = new Vector3d (
      (V1.Y * V2.Z - V1.Z * V2.Y),
      (V1.Z * V2.X - V1.X * V2.Z),
      (V1.X * V2.Y - V1.Y * V2.X));
    return cross;
  }