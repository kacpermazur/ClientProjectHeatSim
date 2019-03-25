import React, { Component } from "react";
import Slider from "react-rangeslider";
import "react-rangeslider/lib/index.css";

class App extends Component {
  constructor(props, context) {
    super(props, context);
    this.state = {
      volume: 0
    };
  }

  handleOnChange = value => {
    this.setState({
      volume: value
    });

    console.log(value);
  };
  render() {
    let { volume } = this.state;
    return (
      <React.Fragment>
        <div>Temp</div>
        <div>Temp</div>
        <div>Temp</div>
        <div>Temp</div>

        <Slider
          value={volume}
          orientation="vertical"
          onChange={this.handleOnChange}
        />
      </React.Fragment>
    );
  }
}

export default App;
