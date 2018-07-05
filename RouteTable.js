import React from "react";
import { routeinfo_get } from "./server";

class RouteTable extends React.Component {
  state = {
    controllers: [],
    seeTable: false
  };

  componentDidMount = () => {
    routeinfo_get()
      .then(resp => {
        this.setState({
          controllers: resp.data,
          seeTable: true
        });
      })
      .catch(err => {
        alert(err);
      });
  };

  render() {
    return (
      <div>
        <div className="col-md-12">
          {this.state.seeTable && (
            <div className="table-responsive mb-20">
              <h2 style={{ marginBottom: "20px" }}>Route Info</h2>
              <table className="table">
                <thead>
                  <tr>
                    <th className="text-center">#</th>
                    <th>Route</th>
                    <th>Class Name</th>
                    <th>Method Name</th>
                    <th>Http Verb</th>
                    <th>Authentication</th>
                    <th>Role</th>
                  </tr>
                </thead>
                <tbody>
                  {this.state.controllers.map((obj, index) => (
                    <tr>
                      <td className="text-center">{index}</td>
                      <td>{obj.route}</td>
                      <td>{obj.className}</td>
                      <td>{obj.methodName}</td>
                      <td>{obj.httpVerb}</td>
                      <td
                        style={!obj.authentication ? { color: "#FF0000" } : {}}
                      >
                        {obj.authentication ? "True" : "False"}
                      </td>
                      <td>{obj.role ? obj.role : "---"}</td>
                    </tr>
                  ))}
                </tbody>
              </table>
            </div>
          )}
        </div>
      </div>
    );
  }
}

export default RouteTable;
