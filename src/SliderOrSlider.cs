  private void RunScript(double S0, double S1, object S2, ref object OS0, ref object OS1, ref object OS2)
  {

    Component.Message = "Omerta Holding";

    if (InputS0 != m_S0_lastvalue)
    {
      m_modified_slider = 0;
    }
    else if (InputS1 != m_S1_lastvalue)
    {
      m_modified_slider = 1;
    }
    else
    {
      m_modified_slider = -1;
    }

    m_S0_lastvalue = InputS0;
    m_S1_lastvalue = InputS1;


    OS0 = InputS0;
    OS1 = InputS1;


    if (m_modified_slider > -1)
    {
      var gh = GrasshopperDocument;
      var cb_delegate = new GH_Document.GH_ScheduleDelegate(Callback);
      if (gh != null && cb_delegate != null)
      {
        gh.ScheduleSolution(1, cb_delegate);
      }
    }

  }


  private int m_modified_slider;
  private double m_S0_lastvalue;
  private double m_S1_lastvalue;

  private IGH_Param m_input_S0;
  private IGH_Param m_input_S1;

  private void Callback(GH_Document doc)
  {
    if (InputS0_IsConnected && m_modified_slider == 0)
    {
      var slider_S1 = _InputS1.Sources[0] as Grasshopper.Kernel.Special.GH_NumberSlider;
      if (slider_S1 != null)
      {
        slider_S1.SetSliderValue(0);
      }
      else
      {
        throw new System.InvalidOperationException("No valid slider S1 is connected!");
      }
    }
    else if (InputS1_IsConnected && m_modified_slider == 1)
    {
      var slider_S0 = _InputS0.Sources[0] as Grasshopper.Kernel.Special.GH_NumberSlider;
      if (slider_S0 != null)
      {
        slider_S0.SetSliderValue(0);
      }
      else
      {
        throw new System.InvalidOperationException("No valid slider S0 is connected!");
      }
    }
    Component.ExpireSolution(false);
  }


  private double InputS0
  {
    get {
      if (InputS0_IsConnected)
      {
        var dat = (GH_Number) m_input_S0.VolatileData.get_Branch(0)[0];
        return dat.Value;
      }
      else { return double.MinValue; }
    }
    set {
      if (InputS0_IsConnected)
      {
        var dat = (GH_Number) m_input_S0.VolatileData.get_Branch(0)[0];
        dat.Value = value;
      }
    }
  }

  private bool InputS0_IsConnected
  {
    get {
      return _InputS0 != null && m_input_S0.SourceCount > 0;
    }
  }


  private IGH_Param _InputS0
  {
    get {
      var ix = Component.Params.IndexOfInputParam("S0");
      if (ix < 0)
        throw new System.ArgumentNullException("Invalid inport 'S0'. Could not be found.");
      m_input_S0 = Component.Params.Input[ix];
      return m_input_S0;
    }
  }

 
  private double InputS1
  {
    get {
      if (InputS1_IsConnected)
      {
        var dat = (GH_Number) m_input_S1.VolatileData.get_Branch(0)[0];
        return dat.Value;
      }
      else { return double.MinValue; }
    }
    set {
      if (InputS1_IsConnected)
      {
        var dat = (GH_Number) m_input_S1.VolatileData.get_Branch(0)[0];
        dat.Value = value;
      }
    }
  }

  private bool InputS1_IsConnected
  {
    get {
      return _InputS1 != null && m_input_S1.SourceCount > 0;
    }
  }

  private IGH_Param _InputS1
  {
    get {
      var ix = Component.Params.IndexOfInputParam("S1");
      if (ix < 0)
        throw new System.ArgumentNullException("Invalid inport 'S1'. Could not be found.");
      m_input_S1 = Component.Params.Input[ix];
      return m_input_S1;
    }
  }

