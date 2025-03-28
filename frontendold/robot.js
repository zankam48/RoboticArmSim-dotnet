let scene, camera, renderer, orbit;
let robotArm = {}; 

// Basic materials for the arm
const materials = {
    base: new THREE.MeshStandardMaterial({ color: 0x444444 }),   
    link: new THREE.MeshStandardMaterial({ color: 0x888888 }),   
    joint: new THREE.MeshStandardMaterial({ color: 0xaa0000 }),  
    gripper: new THREE.MeshStandardMaterial({ color: 0x333333 }) 
};

function init() {
    const container = document.getElementById('robot-container');

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

    // Lights
    const ambientLight = new THREE.AmbientLight(0x404040, 1);
    scene.add(ambientLight);

    const dirLight = new THREE.DirectionalLight(0xffffff, 0.8);
    dirLight.position.set(5, 10, 5);
    dirLight.castShadow = true;
    scene.add(dirLight);

    // Helpers
    const gridHelper = new THREE.GridHelper(10, 10);
    scene.add(gridHelper);
    const axesHelper = new THREE.AxesHelper(2);
    scene.add(axesHelper);

    // Orbit Controls
    orbit = new THREE.OrbitControls(camera, renderer.domElement);
    orbit.enableDamping = true;
    orbit.dampingFactor = 0.05;

    // Create the robotic arm
    createRobotArm();

    // Handle window resizing
    window.addEventListener('resize', onWindowResize);

    // UI controls
    setupControls();

    // Start the render loop
    animate();
}

function onWindowResize() {
    const container = document.getElementById('robot-container');
    camera.aspect = container.clientWidth / container.clientHeight;
    camera.updateProjectionMatrix();
    renderer.setSize(container.clientWidth, container.clientHeight);
}

function animate() {
    requestAnimationFrame(animate);
    orbit.update();
    renderer.render(scene, camera);
}

// Create a more industrial/hobbyist style 6-DOF arm
function createRobotArm() {
    // ------------------------------------------------------
    // 1) BASE PLATFORM (does not move)
    // ------------------------------------------------------
    const baseGeo = new THREE.CylinderGeometry(0.6, 0.6, 0.2, 32);
    const baseMesh = new THREE.Mesh(baseGeo, materials.base);
    baseMesh.receiveShadow = true;
    // Place it so the top is at y=0
    baseMesh.position.y = -0.1;
    scene.add(baseMesh);
    robotArm.base = baseMesh;

    // ------------------------------------------------------
    // 2) JOINT 1 (BASE ROTATION)
    //    - pivot rotates around Y
    // ------------------------------------------------------
    robotArm.joint1Pivot = new THREE.Group();
    // The pivot is at the center of the base's top (y=0)
    robotArm.joint1Pivot.position.set(0, 0, 0);
    baseMesh.add(robotArm.joint1Pivot);

    // A cylinder to visually represent the rotating base
    const rotatingBaseGeo = new THREE.CylinderGeometry(0.3, 0.3, 0.2, 32);
    const rotatingBaseMesh = new THREE.Mesh(rotatingBaseGeo, materials.link);
    rotatingBaseMesh.castShadow = true;
    rotatingBaseMesh.position.y = 0.1;
    robotArm.joint1Pivot.add(rotatingBaseMesh);

    // ------------------------------------------------------
    // 3) JOINT 2 (SHOULDER)
    //    - pivot rotates around X
    // ------------------------------------------------------
    robotArm.joint2Pivot = new THREE.Group();
    // The shoulder pivot is at the top of the rotating base
    robotArm.joint2Pivot.position.y = 0.2;
    robotArm.joint1Pivot.add(robotArm.joint2Pivot);

    // Spherical joint visual
    const joint2Sphere = new THREE.Mesh(new THREE.SphereGeometry(0.08, 16, 16), materials.joint);
    joint2Sphere.castShadow = true;
    robotArm.joint2Pivot.add(joint2Sphere);

    // Link 1 (shoulder link)
    // Cylinder length: 1.0, pivot at bottom
    const link1Length = 1.0;
    const link1Geo = new THREE.CylinderGeometry(0.07, 0.07, link1Length, 16);
    const link1Mesh = new THREE.Mesh(link1Geo, materials.link);
    link1Mesh.castShadow = true;
    // Move so bottom is at pivot, top is at y=link1Length
    link1Mesh.position.y = link1Length / 2;
    robotArm.joint2Pivot.add(link1Mesh);

    // ------------------------------------------------------
    // 4) JOINT 3 (ELBOW)
    //    - pivot rotates around X
    // ------------------------------------------------------
    robotArm.joint3Pivot = new THREE.Group();
    // The elbow pivot is at the top of link1
    robotArm.joint3Pivot.position.y = link1Length;
    robotArm.joint2Pivot.add(robotArm.joint3Pivot);

    // Spherical joint visual
    const joint3Sphere = new THREE.Mesh(new THREE.SphereGeometry(0.07, 16, 16), materials.joint);
    joint3Sphere.castShadow = true;
    robotArm.joint3Pivot.add(joint3Sphere);

    // Link 2 (forearm)
    const link2Length = 1.0;
    const link2Geo = new THREE.CylinderGeometry(0.07, 0.07, link2Length, 16);
    const link2Mesh = new THREE.Mesh(link2Geo, materials.link);
    link2Mesh.castShadow = true;
    link2Mesh.position.y = link2Length / 2;
    robotArm.joint3Pivot.add(link2Mesh);

    // ------------------------------------------------------
    // 5) JOINT 4 (WRIST 1)
    //    - pivot rotates around Y
    // ------------------------------------------------------
    robotArm.joint4Pivot = new THREE.Group();
    // The wrist pivot is at the top of link2
    robotArm.joint4Pivot.position.y = link2Length;
    robotArm.joint3Pivot.add(robotArm.joint4Pivot);

    const joint4Sphere = new THREE.Mesh(new THREE.SphereGeometry(0.06, 16, 16), materials.joint);
    joint4Sphere.castShadow = true;
    robotArm.joint4Pivot.add(joint4Sphere);

    // A short wrist link
    const wristLinkLength = 0.4;
    const wristLinkGeo = new THREE.CylinderGeometry(0.06, 0.06, wristLinkLength, 16);
    const wristLinkMesh = new THREE.Mesh(wristLinkGeo, materials.link);
    wristLinkMesh.castShadow = true;
    wristLinkMesh.position.y = wristLinkLength / 2;
    robotArm.joint4Pivot.add(wristLinkMesh);

    // ------------------------------------------------------
    // 6) JOINT 5 (WRIST 2)
    //    - pivot rotates around X
    // ------------------------------------------------------
    robotArm.joint5Pivot = new THREE.Group();
    robotArm.joint5Pivot.position.y = wristLinkLength;
    robotArm.joint4Pivot.add(robotArm.joint5Pivot);

    const joint5Sphere = new THREE.Mesh(new THREE.SphereGeometry(0.05, 16, 16), materials.joint);
    joint5Sphere.castShadow = true;
    robotArm.joint5Pivot.add(joint5Sphere);

    // Another short link for the second wrist segment
    const wristLink2Length = 0.3;
    const wristLink2Geo = new THREE.CylinderGeometry(0.05, 0.05, wristLink2Length, 16);
    const wristLink2Mesh = new THREE.Mesh(wristLink2Geo, materials.link);
    wristLink2Mesh.castShadow = true;
    wristLink2Mesh.position.y = wristLink2Length / 2;
    robotArm.joint5Pivot.add(wristLink2Mesh);

    // ------------------------------------------------------
    // 7) JOINT 6 (WRIST 3)
    //    - pivot rotates around Y
    // ------------------------------------------------------
    robotArm.joint6Pivot = new THREE.Group();
    robotArm.joint6Pivot.position.y = wristLink2Length;
    robotArm.joint5Pivot.add(robotArm.joint6Pivot);

    const joint6Sphere = new THREE.Mesh(new THREE.SphereGeometry(0.04, 16, 16), materials.joint);
    joint6Sphere.castShadow = true;
    robotArm.joint6Pivot.add(joint6Sphere);

    // ------------------------------------------------------
    // End Effector: Simple Parallel Gripper
    // ------------------------------------------------------
    const gripperGroup = new THREE.Group();
    gripperGroup.position.y = 0;
    robotArm.joint6Pivot.add(gripperGroup);

    // Base of gripper (small cylinder)
    const gripperBaseGeo = new THREE.CylinderGeometry(0.06, 0.06, 0.1, 16);
    const gripperBase = new THREE.Mesh(gripperBaseGeo, materials.gripper);
    gripperBase.castShadow = true;
    gripperBase.rotation.x = Math.PI / 2; // lay cylinder horizontally
    gripperGroup.add(gripperBase);

    // Two jaws (boxes)
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

function setJointAngles(angles) {
    if (angles.joint1 !== undefined) robotArm.joint1Pivot.rotation.y = angles.joint1 * Math.PI / 180;
    if (angles.joint2 !== undefined) robotArm.joint2Pivot.rotation.x = angles.joint2 * Math.PI / 180;
    if (angles.joint3 !== undefined) robotArm.joint3Pivot.rotation.x = angles.joint3 * Math.PI / 180;
    if (angles.joint4 !== undefined) robotArm.joint4Pivot.rotation.y = angles.joint4 * Math.PI / 180;
    if (angles.joint5 !== undefined) robotArm.joint5Pivot.rotation.x = angles.joint5 * Math.PI / 180;
    if (angles.joint6 !== undefined) robotArm.joint6Pivot.rotation.y = angles.joint6 * Math.PI / 180;
}

function setupControls() {
    const joint1Slider = document.getElementById('joint1');
    const joint2Slider = document.getElementById('joint2');
    const joint3Slider = document.getElementById('joint3');
    const joint4Slider = document.getElementById('joint4');
    const joint5Slider = document.getElementById('joint5');
    const joint6Slider = document.getElementById('joint6');

    const joint1Value = document.getElementById('joint1-value');
    const joint2Value = document.getElementById('joint2-value');
    const joint3Value = document.getElementById('joint3-value');
    const joint4Value = document.getElementById('joint4-value');
    const joint5Value = document.getElementById('joint5-value');
    const joint6Value = document.getElementById('joint6-value');

    // update display and arm rotation
    joint1Slider.addEventListener('input', () => {
    joint1Value.textContent = joint1Slider.value + '°';
    setJointAngles({ joint1: parseFloat(joint1Slider.value) });
    });
    joint2Slider.addEventListener('input', () => {
    joint2Value.textContent = joint2Slider.value + '°';
    setJointAngles({ joint2: parseFloat(joint2Slider.value) });
    });
    joint3Slider.addEventListener('input', () => {
    joint3Value.textContent = joint3Slider.value + '°';
    setJointAngles({ joint3: parseFloat(joint3Slider.value) });
    });
    joint4Slider.addEventListener('input', () => {
    joint4Value.textContent = joint4Slider.value + '°';
    setJointAngles({ joint4: parseFloat(joint4Slider.value) });
    });
    joint5Slider.addEventListener('input', () => {
    joint5Value.textContent = joint5Slider.value + '°';
    setJointAngles({ joint5: parseFloat(joint5Slider.value) });
    });
    joint6Slider.addEventListener('input', () => {
    joint6Value.textContent = joint6Slider.value + '°';
    setJointAngles({ joint6: parseFloat(joint6Slider.value) });
    });

    // reset
    document.getElementById('reset').addEventListener('click', () => {
    joint1Slider.value = 0;
    joint2Slider.value = 0;
    joint3Slider.value = 0;
    joint4Slider.value = 0;
    joint5Slider.value = 0;
    joint6Slider.value = 0;
    joint1Value.textContent = '0°';
    joint2Value.textContent = '0°';
    joint3Value.textContent = '0°';
    joint4Value.textContent = '0°';
    joint5Value.textContent = '0°';
    joint6Value.textContent = '0°';
    setJointAngles({
        joint1: 0, joint2: 0, joint3: 0,
        joint4: 0, joint5: 0, joint6: 0
    });
    });
}

document.addEventListener('DOMContentLoaded', init);