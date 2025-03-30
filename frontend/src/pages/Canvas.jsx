import React, { useEffect, useRef } from 'react';
import * as THREE from 'three';
import { OrbitControls } from 'three/examples/jsm/controls/OrbitControls'; 

const Canvas = () => {
  const containerRef = useRef(null); 
  const hasInitialized = useRef(false);

  useEffect(() => {
    console.log("Canvas mounted and init called");
    if (hasInitialized.current) {
      return;
    }
    hasInitialized.current = true;
    
    const materials = {
      base: new THREE.MeshStandardMaterial({ color: 0x444444 }),
      link: new THREE.MeshStandardMaterial({ color: 0x888888 }),
      joint: new THREE.MeshStandardMaterial({ color: 0xaa0000 }),
      gripper: new THREE.MeshStandardMaterial({ color: 0x333333 }),
    };

    let scene, camera, renderer, orbit, robotArm = {};

    function init() {
      const container = containerRef.current; 
      if (!container) return; 

      scene = new THREE.Scene();
      scene.background = new THREE.Color(0xf0f0f0);

      camera = new THREE.PerspectiveCamera(
        75,
        container.clientWidth / container.clientHeight,
        0.1,
        1000
      );
      camera.position.set(3, 3, 3);
      camera.lookAt(new THREE.Vector3(0, 0.5, 0));

      renderer = new THREE.WebGLRenderer({ antialias: true });
      renderer.setSize(container.clientWidth, container.clientHeight);
      renderer.shadowMap.enabled = true;
      container.appendChild(renderer.domElement);

      const ambientLight = new THREE.AmbientLight(0x404040, 1);
      scene.add(ambientLight);
      const dirLight = new THREE.DirectionalLight(0xffffff, 0.8);
      dirLight.position.set(5, 10, 5);
      dirLight.castShadow = true;
      scene.add(dirLight);

      const gridHelper = new THREE.GridHelper(10, 10);
      scene.add(gridHelper);
      const axesHelper = new THREE.AxesHelper(2);
      scene.add(axesHelper);

      orbit = new OrbitControls(camera, renderer.domElement);
      orbit.enableDamping = true;
      orbit.dampingFactor = 0.05;

      createRobotArm();

      window.addEventListener('resize', onWindowResize);

      // UI controls (you'll need to adapt these)
      // setupControls();

      // Start the render loop
      animate();
    }

    function onWindowResize() {
      const container = containerRef.current;
      if (!container) return;

      camera.aspect = container.clientWidth / container.clientHeight;
      camera.updateProjectionMatrix();
      renderer.setSize(container.clientWidth, container.clientHeight);
    }

    function animate() {
      requestAnimationFrame(animate);
      orbit.update();
      renderer.render(scene, camera);
    }

    function createRobotArm() {
      const baseGeo = new THREE.CylinderGeometry(0.6, 0.6, 0.2, 32);
      const baseMesh = new THREE.Mesh(baseGeo, materials.base);
      baseMesh.receiveShadow = true;

      baseMesh.position.y = -0.1;
      scene.add(baseMesh);
      robotArm.base = baseMesh;

      robotArm.joint1Pivot = new THREE.Group();
      robotArm.joint1Pivot.position.set(0, 0, 0);
      baseMesh.add(robotArm.joint1Pivot);

      const rotatingBaseGeo = new THREE.CylinderGeometry(0.3, 0.3, 0.2, 32);
      const rotatingBaseMesh = new THREE.Mesh(rotatingBaseGeo, materials.link);
      rotatingBaseMesh.castShadow = true;
      rotatingBaseMesh.position.y = 0.1;
      robotArm.joint1Pivot.add(rotatingBaseMesh);

      robotArm.joint2Pivot = new THREE.Group();
      robotArm.joint2Pivot.position.y = 0.2;
      robotArm.joint1Pivot.add(robotArm.joint2Pivot);

      const joint2Sphere = new THREE.Mesh(new THREE.SphereGeometry(0.08, 16, 16), materials.joint);
      joint2Sphere.castShadow = true;
      robotArm.joint2Pivot.add(joint2Sphere);

      const link1Length = 1.0;
      const link1Geo = new THREE.CylinderGeometry(0.07, 0.07, link1Length, 16);
      const link1Mesh = new THREE.Mesh(link1Geo, materials.link);
      link1Mesh.castShadow = true;
      link1Mesh.position.y = link1Length / 2;
      robotArm.joint2Pivot.add(link1Mesh);

      robotArm.joint3Pivot = new THREE.Group();
      robotArm.joint3Pivot.position.y = link1Length;
      robotArm.joint2Pivot.add(robotArm.joint3Pivot);

      const joint3Sphere = new THREE.Mesh(new THREE.SphereGeometry(0.07, 16, 16), materials.joint);
      joint3Sphere.castShadow = true;
      robotArm.joint3Pivot.add(joint3Sphere);

      const link2Length = 1.0;
      const link2Geo = new THREE.CylinderGeometry(0.07, 0.07, link2Length, 16);
      const link2Mesh = new THREE.Mesh(link2Geo, materials.link);
      link2Mesh.castShadow = true;
      link2Mesh.position.y = link2Length / 2;
      robotArm.joint3Pivot.add(link2Mesh);

      robotArm.joint4Pivot = new THREE.Group();
      robotArm.joint4Pivot.position.y = link2Length;
      robotArm.joint3Pivot.add(robotArm.joint4Pivot);

      const joint4Sphere = new THREE.Mesh(new THREE.SphereGeometry(0.06, 16, 16), materials.joint);
      joint4Sphere.castShadow = true;
      robotArm.joint4Pivot.add(joint4Sphere);

      const wristLinkLength = 0.4;
      const wristLinkGeo = new THREE.CylinderGeometry(0.06, 0.06, wristLinkLength, 16);
      const wristLinkMesh = new THREE.Mesh(wristLinkGeo, materials.link);
      wristLinkMesh.castShadow = true;
      wristLinkMesh.position.y = wristLinkLength / 2;
      robotArm.joint4Pivot.add(wristLinkMesh);
      robotArm.joint5Pivot = new THREE.Group();
      robotArm.joint5Pivot.position.y = wristLinkLength;
      robotArm.joint4Pivot.add(robotArm.joint5Pivot);

      const joint5Sphere = new THREE.Mesh(new THREE.SphereGeometry(0.05, 16, 16), materials.joint);
      joint5Sphere.castShadow = true;
      robotArm.joint5Pivot.add(joint5Sphere);

      const wristLink2Length = 0.3;
      const wristLink2Geo = new THREE.CylinderGeometry(0.05, 0.05, wristLink2Length, 16);
      const wristLink2Mesh = new THREE.Mesh(wristLink2Geo, materials.link);
      wristLink2Mesh.castShadow = true;
      wristLink2Mesh.position.y = wristLink2Length / 2;
      robotArm.joint5Pivot.add(wristLink2Mesh);

      robotArm.joint6Pivot = new THREE.Group();
      robotArm.joint6Pivot.position.y = wristLink2Length;
      robotArm.joint5Pivot.add(robotArm.joint6Pivot);

      const joint6Sphere = new THREE.Mesh(new THREE.SphereGeometry(0.04, 16, 16), materials.joint);
      joint6Sphere.castShadow = true;
      robotArm.joint6Pivot.add(joint6Sphere);

      const gripperGroup = new THREE.Group();
      gripperGroup.position.y = 0;
      robotArm.joint6Pivot.add(gripperGroup);

      const gripperBaseGeo = new THREE.CylinderGeometry(0.06, 0.06, 0.1, 16);
      const gripperBase = new THREE.Mesh(gripperBaseGeo, materials.gripper);
      gripperBase.castShadow = true;
      gripperBase.rotation.x = Math.PI / 2; 
      gripperGroup.add(gripperBase);

      const jawGeo = new THREE.BoxGeometry(0.02, 0.08, 0.02);
      const jawLeft = new THREE.Mesh(jawGeo, materials.gripper);
      jawLeft.position.set(0.04, 0, 0);
      jawLeft.castShadow = true;
      gripperGroup.add(jawLeft);

      const jawRight = new THREE.Mesh(jawGeo, materials.gripper);
      jawRight.position.set(-0.04, 0, 0);
      jawRight.castShadow = true;
      gripperGroup.add(jawRight);
    }

    // function setJointAngles(angles) {
    //   if (angles.joint1 !== undefined) robotArm.joint1Pivot.rotation.y = angles.joint1 * Math.PI / 180;
    //   if (angles.joint2 !== undefined) robotArm.joint2Pivot.rotation.x = angles.joint2 * Math.PI / 180;
    //   if (angles.joint3 !== undefined) robotArm.joint3Pivot.rotation.x = angles.joint3 * Math.PI / 180;
    //   if (angles.joint4 !== undefined) robotArm.joint4Pivot.rotation.y = angles.joint4 * Math.PI / 180;
    //   if (angles.joint5 !== undefined) robotArm.joint5Pivot.rotation.x = angles.joint5 * Math.PI / 180;
    //   if (angles.joint6 !== undefined) robotArm.joint6Pivot.rotation.y = angles.joint6 * Math.PI / 180;
    // }

    init();

    return () => {
      window.removeEventListener('resize', onWindowResize);
      if (containerRef.current && renderer?.domElement) {
        containerRef.current.removeChild(renderer.domElement);
      }
      // hasInitialized.current = false;
    };
    
  }, []);

  return <div id="robot-container" ref={containerRef} style={{ width: '100vw', height: '100vh' }} />;
};

export default Canvas;