import * as axios from "axios";

const URL_PREFIX = "";

export function routeinfo_get() {
  return axios.get(URL_PREFIX + "/api/route-table");
}
