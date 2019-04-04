import React, { Component } from "react";

import InteractHolder from "./component/Interacts/InteractHolder";
import RenderView from "./component/RenderView/RenderView";
import ValueDisplay from "./component/ValueDisplay/ValueDisplay";

class App extends Component {
  render() {
    let props = {
      TemptureTextAir: "test1",
      DayText: "test2",
      TemptureTextWall: "test3"
    };

    return (
      <React.Fragment>
        <center>
          <RenderView />
          <ValueDisplay TemptureTextAir={props} />
          <InteractHolder />
        </center>
      </React.Fragment>
    );
  }
}

export default App;
