rivate void RunScript(bool reset, int size, ref object A)
  {
    Component.Message = "Bouncing Ball";

    //Initialize bob
    if (reset || b == null){
      b = new Bob[size];
      for (int i = 0; i < size; i++){
        b[i] = new Bob(i * 20, 100, i * 10);
      }
    }

    //Incerement bob
    for (int i = 0; i < size; i++){
      b[i].step();
    }


    //Display bob
    pts.Clear();
    for (int i = 0; i < size; i++){
      pts.Add(b[i].pt());
    }


    A = pts;


  }

  // <Custom additional code> 
  Bob[] b;
  List<Point3d> pts = new List<Point3d>();

  class Bob{

    public Vector3d loc;
    public Vector3d vel;

    //Constructor
    public Bob(double x, double y, double z){
      loc = new Vector3d(x, y, z);
      vel = new Vector3d(5, z / 25, 0);
    }

    //Display
    public Point3d pt(){
      Point3d point = new Point3d(loc.X, loc.Y, loc.Z);
      return point;
    }

    //Increment
    public void step(){

      loc += vel;

      //Boundary
      if( (loc.X > 1280) || ( loc.X < 0) ){
        vel.X *= -1;
      }
      if( (loc.Y > 720 ) || (loc.Y < 0) ){
        vel.Y *= -1;
      }

      if( (loc.Z > 200 ) || (loc.Z < 0) ){
        vel.Z *= -1;

      }
    }

  }