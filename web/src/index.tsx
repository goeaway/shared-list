import React from "react";
import { render } from "react-dom";
import App from "./components/app";
import { BrowserRouter as Router } from "react-router-dom";

const root = document.createElement("div");
root.id = "shared-list-root";
document.body.appendChild(root);

render(
    <Router>
        <App />
    </Router>,
    root
);