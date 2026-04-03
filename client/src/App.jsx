import { Navigate, Route, Routes } from 'react-router-dom';
import LoginPage from './pages/LoginPage';
import RegisterPage from './pages/RegisterPage';
import ChatPage from './pages/ChatPage';
import { useAuth } from './contexts/AuthContext';

function ProtectedRoute({ children }) {
  const { token } = useAuth();
  return token ? children : <Navigate to="/login" replace />;
}

function PublicRoute({ children }) {
  const { token } = useAuth();
  return token ? <Navigate to="/chat" replace /> : children;
}

export default function App() {
  return (
    <Routes>
      <Route path="/" element={<Navigate to="/chat" replace />} />
      <Route
        path="/login"
        element={(
          <PublicRoute>
            <LoginPage />
          </PublicRoute>
        )}
      />
      <Route
        path="/register"
        element={(
          <PublicRoute>
            <RegisterPage />
          </PublicRoute>
        )}
      />
      <Route
        path="/chat"
        element={(
          <ProtectedRoute>
            <ChatPage />
          </ProtectedRoute>
        )}
      />
    </Routes>
  );
}
