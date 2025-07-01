import { Injectable, signal, WritableSignal } from '@angular/core';
import * as signalR from '@microsoft/signalr';

@Injectable({
  providedIn: 'root',
})
export class Notification {
  hubConnection: signalR.HubConnection | null = null;
  latestMessage: WritableSignal<string | null> = signal(null);
  constructor() {
    this.startConnection();
    this.hubConnection?.onclose((err) => {
      console.warn('Connection closed', err);
    });
    window.addEventListener('beforeunload', () => {
      this.hubConnection?.stop();
    });
  }
  startConnection(): void {
    console.log('Starting SignalR connection...');
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl('http://localhost:5243/notifyHub', { withCredentials: true })
      .withAutomaticReconnect()
      .build();

    this.hubConnection
      .start()
      .then(() => console.log('SignalR connected'))
      .catch((err) => console.log('SignalR error:', err));

    this.hubConnection.on('ReceiveNotification', (data) => {
      console.log('Received message:', data);
      this.latestMessage.set(data);
    });
  }
}
