import grpc from 'k6/net/grpc';
import { check, sleep } from 'k6';
import { htmlReport } from "https://raw.githubusercontent.com/benc-uk/k6-reporter/main/dist/bundle.js";

export const options = {
  vus: 10,
  duration: '30s',
};


const client = new grpc.Client();
client.load(['definitions'], 'greet.proto');

export default () => {
  if (__ITER == 0) {// only on the first iteration
    client.connect('localhost:25001', {
      plaintext: true
    });
  }

  const data = { name: 'Gustavo' };
  const response = client.invoke('greet.Greeter/SayHello', data);

  check(response, {
    'status is OK': (r) => r && r.status === grpc.StatusOK,
  });

};

export function handleSummary(data) {
  return {
    "summarygrpc.html": htmlReport(data),
  };
}

export function teardown(data) {
  client.close();
}