import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { HttpTransportType, HubConnection, HubConnectionBuilder, LogLevel } from '@aspnet/signalr';
import { Account } from '../models/Account';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent {
  private _hubConnection!: HubConnection;
  amount: number = 0;
  baseUrl = environment.baseUrl;

  account: Account = {
    balance: 0,
    state: '',
    owner: ''
  };

  constructor(private http: HttpClient) {
    this.createConnection();
    this.startConnection();
    this.listen();
  }

  deposit() {
    if (this.amount <= 0) return;

    this.http
      .post(`${this.baseUrl}/account/deposit/${this.amount}`, {})
      .subscribe(data => console.log(data));
  }

  withdraw() {
    if (this.amount <= 0) return;

    this.http
      .post(`${this.baseUrl}/account/withdraw/${this.amount}`, {})
      .subscribe(data => console.log(data));
  }

  payInterest() {
    this.http
      .post(`${this.baseUrl}/account/payinterest`, {})
      .subscribe(data => console.log(data));
  }

  getClass(state: string) {
    switch (state) {
      case 'Gold':
        return 'alert alert-success';
      case 'Silver':
        return 'alert alert-info';
      case 'Red':
        return 'alert alert-danger';
      default:
        return 'alert alert-light';
    }
  }

  private createConnection() {
    this._hubConnection = new HubConnectionBuilder()
      .configureLogging(LogLevel.Debug)
      .withUrl(`${this.baseUrl}/accountHub`, {
        skipNegotiation: true,
        transport: HttpTransportType.WebSockets
      })
      .build();
  }

  private startConnection() {
    this._hubConnection
      .start()
      .then(() => {
        this.connectToHub();
      })
      .catch(() => {
        setTimeout(() => this.startConnection(), 5000);
      });
  }

  private connectToHub() {
    this._hubConnection.invoke('ConnectToHub');
  }

  private listen() {
    this._hubConnection.on('ChangedState', (data: any) => {
      console.log("ChangedState", data);
      this.account.balance = data.balance;
      this.account.state = data.state;
      this.account.owner = data.owner;
    });
  }
}
