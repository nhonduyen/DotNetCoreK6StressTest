import http from 'k6/http';
import { sleep } from 'k6';

export default function () {
    http.get('https://localhost:7085/api/product/getall');
    sleep(1);
}