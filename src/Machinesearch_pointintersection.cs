  private void RunScript(int N, double x, double y, double x0, double y0, double x1, double y1, object void0, int fate, int what, ref object randPtsList, ref object boundaryRectangle, ref object searchRectangle, ref object filteredPointsList)
  {
    List<Point3d> randPts = new List<Point3d>();

    for(int i = 0; i < N;i++){
      Point3d p = new Point3d(rand.NextDouble(-x / 2.0, x / 2.0), rand.NextDouble(-y / 2.0, y / 2.0), 0);
      randPts.Add(p);
    }
    randPtsList = randPts;

    Interval intX = new Interval(-x / 2.0, x / 2.0);
    Interval intY = new Interval(-y / 2.0, y / 2.0);
    Rectangle3d rect = new Rectangle3d(Plane.WorldXY, intX, intY);
    boundaryRectangle = rect;

    if(x0 == x1) x1 += 1; if(y0 == y1) y1 += 1;
    Rectangle3d rectS = new Rectangle3d(Plane.WorldXY, new Point3d(x0, y0, 0), new Point3d(x1, y1, 0));
    searchRectangle = rectS;

    List<Point3d> inclPts = FindPoints(randPts, rectS, what);
    filteredPointsList = inclPts;

  }

  public List<Point3d> FindPoints(List<Point3d> randPts, Rectangle3d rect, int what){
    double tol = RhinoDoc.ActiveDoc.ModelAbsoluteTolerance;
    Curve c = rect.ToNurbsCurve();

    List<Point3d> inclPts = new List<Point3d>();
    foreach(Point3d p in randPts){
      PointContainment pc = c.Contains(p, Plane.WorldXY, tol);
      switch(what){
        case 1: if(pc == PointContainment.Inside) inclPts.Add(p); break;
        case 2: if(pc == PointContainment.Outside) inclPts.Add(p); break;
      }
    }
    return inclPts;
  }

  RangedRandom rand = new RangedRandom();
  class RangedRandom : System.Random{

    public RangedRandom(): base(){}
    public RangedRandom(int seed): base(seed){}

    public double NextDouble(double max){
      return NextDouble() * max;
    }

    public double NextDouble(double min, double max){
      return (max - min) * NextDouble() + min;
    }
  }