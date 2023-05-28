import http from 'k6/http';
import { sleep, check } from 'k6';
import { htmlReport } from "https://raw.githubusercontent.com/benc-uk/k6-reporter/main/dist/bundle.js";

export const options = {
  vus: 10,
  duration: '30s',
};

export default function () {
  const res = http.get('https://localhost:25000/greeter/gustavo');
  check(res, {
    'is status 200': (r) => r.status === 200,
  });
}

export function handleSummary(data) {
  return {
    "summaryjsonminimalapi.html": htmlReport(data),
  };
}
