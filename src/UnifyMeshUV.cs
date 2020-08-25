private void RunScript(Surface x, ref object A)
  {
    Component.Message = "Omerta Holding";

    double pi = 3.1415926535897932384626433832795;

    Rhino.Geometry.Point3d p;
    Rhino.Geometry.Vector3d[] UV;
    // Evaluating surfaces at center (median of domain start and end)
    x.Evaluate((x.Domain(0).T0 + x.Domain(0).T1) / 2, (x.Domain(1).T0 + x.Domain(1).T1) / 2, 1, out p, out UV);

    // U, V, WorldX, WorldY vectors
    Rhino.Geometry.Vector3d U = UV[0];
    Rhino.Geometry.Vector3d V = UV[1];
    Rhino.Geometry.Vector3d X = Rhino.Geometry.Plane.WorldXY.XAxis;
    Rhino.Geometry.Vector3d Y = Rhino.Geometry.Plane.WorldXY.YAxis;

    // Unitizing vectors to make tests
    U.Unitize();
    V.Unitize();

    if (Math.Abs(U.X) < Math.Abs(V.X)){ // If this is "true" it means V isocurves are more suitable to be aligned with World X axis than U; U and V needs to be swapped
      x.Transpose(true); //Swapping UV
      if (Vector3d.VectorAngle(V, X) > pi / 2){ // Flipping new U if angle with X is larger than 90°
        x.Reverse(0, true);}
      if (Vector3d.VectorAngle(U, Y) > pi / 2){ // Flipping new V if angle with Y is larger than 90°
        x.Reverse(1, true);}
    }
    else{ // U an V doesn't need to be swapped
      if (Vector3d.VectorAngle(U, X) > pi / 2){ // Flipping U if angle with X is larger than 90°
        x.Reverse(0, true);}
      if (Vector3d.VectorAngle(V, Y) > pi / 2){ // Flipping V if angle with Y is larger than 90°
        x.Reverse(1, true);}
    }
    A = x;
  }