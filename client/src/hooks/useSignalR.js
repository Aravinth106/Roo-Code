import { useEffect, useRef, useState } from 'react';
import { HubConnectionBuilder, LogLevel } from '@microsoft/signalr';

export default function useSignalR(token, userId, onReceiveMessage, onOnlineUsers) {
  const [connected, setConnected] = useState(false);
  const connectionRef = useRef(null);

  useEffect(() => {
    if (!token || !userId) return undefined;

    const connection = new HubConnectionBuilder()
      .withUrl(`${import.meta.env.SIGNALR_URL || 'http://localhost:5000'}/chatHub`, {
        accessTokenFactory: () => token,
      })
      .withAutomaticReconnect()
      .configureLogging(LogLevel.Information)
      .build();

    connection.on('ReceiveMessage', onReceiveMessage);
    connection.on('OnlineUsers', onOnlineUsers);

    const start = async () => {
      await connection.start();
      await connection.invoke('JoinUserRoom', userId);
      setConnected(true);
    };

    start().catch((error) => {
      console.error('SignalR connection failed', error);
      setConnected(false);
    });

    connectionRef.current = connection;

    return () => {
      connection.stop();
      setConnected(false);
    };
  }, [token, userId, onReceiveMessage, onOnlineUsers]);

  const sendMessage = async (receiverId, content) => {
    if (!connectionRef.current || !connected) return;
    await connectionRef.current.invoke('SendMessage', receiverId, content);
  };

  return { connected, sendMessage };
}
