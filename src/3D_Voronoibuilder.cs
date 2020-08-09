public int FindClosestIndex (List<Point3d> pList, Point3d p){Rhino.Collections.Point3dList pts = new Point3dList(pList);
  return pts.ClosestIndex(p);
}

public void ComputeConnectivityAndPoints(List < Line > lines, ref List < Point3d > ptsList, ref DataTree < int > LPConnTree,
ref DataTree < int > PLConnTree, ref DataTree < int > PPConnTree){

  int ptsCount = 0;
  for(int i = 0; i < lines.Count;i++){
    Line line = lines[i];
    Point3d ps = line.PointAt(0); Point3d pe = line.PointAt(1);
    ptsList.Add(ps); ptsList.Add(pe);

    int psIndex = ptsCount;
    int peIndex = ptsCount + 1;

    LPConnTree.Add(psIndex, new GH_Path(i));
    LPConnTree.Add(peIndex, new GH_Path(i));

    PLConnTree.Add(i, new GH_Path(psIndex));
    PLConnTree.Add(i, new GH_Path(peIndex));

    PPConnTree.Add(peIndex, new GH_Path(psIndex));
    PPConnTree.Add(psIndex, new GH_Path(peIndex));
    ptsCount += 2;
  }
}

private List<Brep> Voronoi3d(List<Point3d> list, Box box, double inflate) {
  checked {
    for (int i = list.Count - 1; i >= 0; i += -1) {
      if (!list[i].IsValid)list.RemoveAt(i);
    }
    if (list.Count == 0)return null;
    Box validBox;
    double tol = RhinoDoc.ActiveDoc.ModelAbsoluteTolerance;
    if (box.IsValid && box.Z.T1 - box.Z.T0 > tol) validBox = box;
    else {
      Print("Flat box: inflating ({0})", inflate);
      Point3dList point3dList = new Point3dList(list);
      validBox = new Box(point3dList.BoundingBox);
      if (validBox.Volume == 0.0)  validBox.Inflate(inflate, inflate, inflate);
    }

    List<Brep> list2 = new List<Brep>();
    int num = list.Count - 1;
    for (int j = 0; j <= num; j++) {
      Grasshopper.Kernel.Geometry.Voronoi.Cell3 cell = new Grasshopper.Kernel.Geometry.Voronoi.Cell3(list[j], validBox);
      try {
        List<Point3d> list3 = new List<Point3d>(list.Count - 1);
        int arg_10F_0 = 0;
        int num2 = list.Count - 1;
        for (int k = arg_10F_0; k <= num2; k++) {
          if (j != k) {
            if (!(list[j].DistanceTo(list[k]) < 1E-06))list3.Add(list[k]);
          }
        }
        cell.Slice(list3);

        if (cell.Facets.Count == 0)list2.Add(null);
        else {
          Brep brep = cell.ToBrep();
          if (brep != null && !brep.IsSolid){
            Brep brep2 = brep.CapPlanarHoles(0.001);
            if (brep2 != null) brep = brep2;
          }
          list2.Add(brep);
        }
      }
      catch (System.Exception ex){Component.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, ex.ToString());}
    }
    return list2;
  }
}

if(!enable){Print(" "); Print("Asta la vista baby");return;}
List<Line> lines = linesTree.AllData().ToList();
lines = lines.Where(x => x != null).ToList();
if(!lines.Any()){Print(" "); Print("Adios amigos");return;}

List < Point3d > ptsList = new List<Point3d>();
DataTree<int> LPConnTree = new  DataTree<int>();
DataTree<int> PLConnTree = new  DataTree<int>();
DataTree<int> PPConnTree = new  DataTree<int>();
ComputeConnectivityAndPoints(lines, ref ptsList, ref  LPConnTree, ref  PLConnTree, ref PPConnTree);
LPTree = LPConnTree; //>>out
PLTree = PLConnTree; //>>out
PPTree = PPConnTree; //>>out

List<Brep> breps = Voronoi3d(ptsList, volumeVorBox, 0);
orthoPts3DList = ptsList;       //>>out
orthoConnections3DList = lines; //>>out

DataTree<Brep> brepsTree = new DataTree<Brep>();
foreach(GH_Path path in LPConnTree.Paths){
  try{
    int index0 = LPConnTree.Branch(path)[0];brepsTree.Add(breps[index0], new GH_Path(path));
    int index1 = LPConnTree.Branch(path)[1];brepsTree.Add(breps[index1], new GH_Path(path));
  }
  catch{Print("Pair tree failure at: {0}", path.ToString(true));}
}

voronoiBreps = brepsTree;       //>>out
voronoiBrepsCount = breps.Count;//>>out

Print(" ");
Print("Good");
Print("Wrong", breps.Count);
