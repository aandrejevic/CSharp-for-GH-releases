 private void RunScript(bool reset, bool run, ref object A)
  {

    Component.Message = "Walker";

    if(reset){
      pt = new Point3d(0, 0, 0);
      points.Clear();
    }else if (run){
      points.Add(pt);
      step();
      Component.ExpireSolution(true);
    }

    A = points;


  }

  // <Custom additional code> 

  Random random = new Random();
  Point3d pt = new Point3d(0, 0, 0);
  List<Point3d> points = new List<Point3d>();

  void step(){
    pt.X += random.Next(-1, 2) * 10;
    pt.Y += random.Next(-1, 2) * 10;
    pt.Z += random.Next(-1, 2) * 10;
  }
