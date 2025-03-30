import React, { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';
import './Dashboard.css';

const Dashboard = () => {
  const { logout } = useAuth();
  const navigate = useNavigate();
  const [jointAngles, setJointAngles] = useState({
    joint1: 0, joint2: 0, joint3: 0,
    joint4: 0, joint5: 0, joint6: 0
  });

  useEffect(() => {
    const savedAngles = localStorage.getItem('robotAngles');
    if (savedAngles){
      setJointAngles(JSON.parse(savedAngles));
    }
  }, []);

  useEffect(() => {
    localStorage.setItem('robotAngles', JSON.stringify(jointAngles));
  }, [jointAngles]);

  const handleLogout = () => {
    logout();
    navigate('/');
  };

  const goToCanvas = () => {
    navigate('/canvas');
  };

  const handleSliderChange = (joint, value) => {
    const updated = { ...jointAngles, [joint]: parseFloat(value) };
    setJointAngles(updated);

    // bisa juga ke signalr
    window.dispatchEvent(new CustomEvent("jointUpdate", { detail: updated }));
  };

  return (
    <div className="dashboard-container">
      <div className="dashboard-content"> 
        <div className="dashboard-card">
          <h1>Welcome to the Robot Arm Dashboard</h1>
          <p>Control your robotic arm and see live movement data.</p>
          <div className="dashboard-actions">
            <button onClick={goToCanvas}>Open Canvas</button>
            <button className="logout-btn" onClick={handleLogout}>Logout</button>
          </div>
        </div>
        <div className="sliders-section">
          {Object.entries(jointAngles).map(([joint, value]) => (
            <div className="slider" key={joint}>
              <label>{joint.toUpperCase()}: {value}°</label>
              <input
                type="range"
                min="-180"
                max="180"
                value={value}
                onChange={(e) => handleSliderChange(joint, e.target.value)}
              />
            </div>
          ))}
        </div>
      </div>
    </div>
  );
};

export default Dashboard;