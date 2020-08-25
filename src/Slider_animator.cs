  private void RunScript(System.Object Slider , int Steps, bool Execute)
  {
    if (!Execute)
    {
      _step = 0;
      _steps = 0;
      _slider = null;
      return;
    }

    if (_slider == null)
    {
      _step = 0;
      _steps = Steps;
      _slider = Component.Params.Input[0].Sources[0] as Grasshopper.Kernel.Special.GH_NumberSlider;
      if (_slider == null)
        throw new InvalidCastException("Source of the Slider input is not a Slider");
    }

    if (_step <= _steps)
      GrasshopperDocument.ScheduleSolution(1, SolutionCallback);
  }

  // <Custom additional code> 
  private int _step;
  private int _steps;
  private Grasshopper.Kernel.Special.GH_NumberSlider _slider;

  private void SolutionCallback(GH_Document document)
  {
    if (_slider == null)
      return;

    double percent = (double) _step / _steps;
    int tick = (int) (_slider.TickCount * percent);
    _slider.TickValue = tick;
    _slider.ExpireSolution(false);

    _step++;
    if (_step > _steps)
      return;
  }