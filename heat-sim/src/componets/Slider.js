import React, { Component } from "react";
import PropTypes from "prop-types";

class Slider extends Component {
  static propTypes = {
    value: PropTypes.number,
    min: PropTypes.number,
    max: PropTypes.number,
    step: PropTypes.number
  };

  render() {
    return (
      <div>
        <div>teste</div>
      </div>
    );
  }
}

export default Slider;
