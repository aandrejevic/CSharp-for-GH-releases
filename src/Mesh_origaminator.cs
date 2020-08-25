  private void RunScript(Mesh mesh, int y, ref object A, ref object B, ref object C)
  {

    List < Plane > planes = new List<Plane>(meshPlanes(mesh).ToList());
    List < Mesh > meshes = new List<Mesh>(){mesh};




    for(int i = 0; i < y; i++){
      Mesh tile = mesh.DuplicateMesh();
      meshes.Add(tile);

      Plane[] tempPlanes = meshPlanes(tile);

      Plane temp = tempPlanes[0];
      temp.Flip();
      temp.Rotate(Math.PI / 2, temp.ZAxis);



      int z = random.Next(1, 3);
      if(i % 4 == 0)
        tile.Transform(Transform.PlaneToPlane(temp, planes[planes.Count - 2]));
      else if (i % 4 == 1)
        tile.Transform(Transform.PlaneToPlane(temp, planes[planes.Count - 3]));
      else if (i % 4 == 2)
        tile.Transform(Transform.PlaneToPlane(temp, planes[planes.Count - 4]));
      else if (i % 4 == 3)
        tile.Transform(Transform.PlaneToPlane(temp, planes[planes.Count - 4]));


      var newPlanes = meshPlanes(tile);

      for(int j = 0; j < newPlanes.Length; j++)
        if(j != 0)
          planes.Add(newPlanes[j]);
    }




    A = planes;
    B = meshes;

