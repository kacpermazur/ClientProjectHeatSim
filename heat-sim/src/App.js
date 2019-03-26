import React, { Component } from "react";

import InteractHolder from "./component/Interacts/InteractHolder";
import RenderView from "./component/RenderView/RenderView";

class App extends Component {
  render() {
    return (
      <React.Fragment>
        <center>
          <RenderView />
          <div>temp</div>
          <InteractHolder />
        </center>
      </React.Fragment>
    );
  }
}

export default App;
