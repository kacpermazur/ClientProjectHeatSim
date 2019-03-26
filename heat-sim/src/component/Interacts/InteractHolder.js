import React, { Component } from "react";

import Slider from "react-rangeslider";
import "react-rangeslider/lib/index.css";
import "../Interacts/InteractHolder.css";

class InteractHolder extends Component {
  state = {
    Tempture: 0,
    Time: 0,
    WallType: 0
  };

  handleTemptureChange = value => {
    this.setState({
      Tempture: value
    });

    console.log("Temp: ", value);
  };

  handleTimeChange = value => {
    this.setState({
      Time: value
    });

    console.log("Time: ", value);
  };

  handleWallChange = value => {
    const types = ["oldWall", "newWallUN", "newWallIN"];

    this.setState({
      WallType: value
    });

    console.log("WallType: ", types[value]);
  };

  render() {
    let { Tempture, Time, WallType } = this.state;

    return (
      <div className="InteractContainer">
        <div className="SliderHolder">
          <Slider
            min={20}
            max={40}
            value={Tempture}
            onChange={this.handleTemptureChange}
          />
          <Slider
            Slider
            min={1}
            max={3}
            value={Time}
            onChange={this.handleTimeChange}
          />
          <Slider
            Slider
            min={0}
            max={2}
            step={1}
            value={WallType}
            onChange={this.handleWallChange}
          />
        </div>
        <button className="StartButton">Start Sim</button>
      </div>
    );
  }
}

export default InteractHolder;
