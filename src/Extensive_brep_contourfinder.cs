private void RunScript(DataTree<Brep> brepTree, object void1, object void2, List<double> pV, object void3, object void4, int dir, bool report, double D, object void5, int boxPolicy, int branches, ref object boxesWireAsTree, ref object contoursAsTree, ref object boxesOrigins, ref object boxesCutAxis)
  {


    Point3d orig = new Point3d (0, 0, 0);
    Plane boxPlane = new Plane (orig, new Vector3d(pV[0], pV[1], pV[2]));

    DataTree<Curve> contoursTree = new DataTree<Curve>();
    DataTree<Curve> boxesWire = new DataTree<Curve>();
    boxOrigins.Clear(); boxAxis.Clear();

    Box unionBox = Box.Empty;
    DataTree<Box> boxesTree = new DataTree<Box>();
    ComputeBoxes(boxPolicy, brepTree, boxPlane, ref unionBox, ref boxesTree);

    ComputeContours(brepTree, boxesTree, unionBox, boxPolicy, D, dir, ref contoursTree, ref boxesWire, branches, report);
    boxesWireAsTree = boxesWire;   //>>out
    contoursAsTree = contoursTree; //>>out

    boxesOrigins = boxOrigins;     //>>out
    boxesCutAxis = boxAxis;        //>>out

    Print(" ");
    Print("Made: {0} contours in {1} branches (explain that)", contoursTree.DataCount, contoursTree.BranchCount);

  }

  // <Custom additional code> 

  public List<Box> boxesList;

  public void ComputeBoxes(int boxPolicy, DataTree < Brep > brepTree, Plane boxPlane, ref Box unionBox, ref DataTree<Box> boxes){
    bool firstBox = true;
    foreach(GH_Path path in brepTree.Paths){
      List<Brep> breps = brepTree.Branch(path).Where(x => x != null).ToList();
      if(!breps.Any())continue;
      for(int i = 0; i < breps.Count;i++){
        Brep brep = breps[i];
        Box box; brep.GetBoundingBox(boxPlane, out box);
        if(boxPolicy == 1) boxes.Add(box, new GH_Path(path));
        else {
          if(firstBox){unionBox = box; firstBox = false;}
          else{
            List<Point3d> pts = box.GetCorners().ToList();
            foreach(Point3d p in pts) unionBox.Union(p);
          }
        }
      }
    }
  }

  public Line boxLine;
  public List<Point3d> boxOrigins = new List<Point3d>();
  public List<Vector3d> boxAxis = new List<Vector3d>();

  public void ComputePlanes(Box box, double D, int dir, ref List<Plane> planes){
    Point3d[] bc = box.GetCorners();

    switch(dir){
      case 1: boxLine = new Line(bc[0], bc[1]); break;
      case 2: boxLine = new Line(bc[0], bc[3]); break;
      case 3: boxLine = new Line(bc[0], bc[4]); break;
    }

    boxOrigins.Add(bc[0]); boxAxis.Add(boxLine.Direction);

    Curve crv = boxLine.ToNurbsCurve();
    Vector3d v = boxLine.Direction;
    Point3d[] pts; crv.DivideByLength(D, true, out pts);
    if(pts != null ){
      foreach(Point3d p in pts){
        Plane plane = new Plane (p, v);
        planes.Add(plane);
      }
    }
  }


  public void  ComputeContours(DataTree < Brep > brepTree, DataTree < Box > boxesTree, Box unionBox, int boxPolicy, double D, int dir,
  ref  DataTree<Curve> contoursTree, ref DataTree<Curve> boxesWire, int branches, bool report){

    List<Plane> planes = new List<Plane>();
    bool computedOnce = false;

    foreach(GH_Path path in brepTree.Paths){
      List<Brep> breps = brepTree.Branch(path).Where(x => x != null).ToList();
      if(!breps.Any())continue;
      if(boxPolicy == 1)  boxesList = boxesTree.Branch(path).ToList();
      for(int i = 0; i < breps.Count;i++){
        Brep brep = breps[i];
        if(boxPolicy == 1){
          Box box = boxesList[i];
          boxesWire.AddRange(box.ToBrep().GetWireframe(-1), new GH_Path(path.AppendElement(i)));
          planes.Clear();
          ComputePlanes(box, D, dir, ref planes);
        }
        else{
          if(!computedOnce){
            ComputePlanes(unionBox, D, dir, ref planes);
            boxesWire.AddRange(unionBox.ToBrep().GetWireframe(-1), new GH_Path(0));
            computedOnce = true;
          }
        }
        if(planes.Any()){
          int contoursCount = 0;
          for(int j = 0; j < planes.Count;j++){
            Plane plane = planes[j];
            List<Curve> contours = Brep.CreateContourCurves(brep, plane).Where(x => x != null).ToList();
            if(contours.Any()){
              GH_Path pathN = path; pathN = pathN.AppendElement(i);
              if(branches == 1) pathN = pathN.AppendElement(j);
              contoursTree.AddRange(contours, new GH_Path(pathN));
              contoursCount += contours.Count;
            }
          }
          if(report) Print("For brep: {0}[{1}] contours: {2}", path.ToString(true), i, contoursCount);
        }
      }
    }
  }