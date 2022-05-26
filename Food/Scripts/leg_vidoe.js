let videofeed;
let posenet;
let poses = [];
let started = false;
var audio = document.getElementById("audioElement");
var stage2 = "down";
var count = 0;
var c = 0;
var t;

// p5.js setup() function to set up the canvas for the web cam video stream
function setup() {
    //creating a canvas by giving the dimensions
    const canvas = createCanvas(400, 450);
    canvas.parent("video");

    videofeed = createCapture(VIDEO);
    videofeed.size(width, height);
    console.log("setup");

    // setting up the poseNet model to feed in the video feed.
    posenet = ml5.poseNet(videofeed);

    posenet.on("pose", function (results) {
        poses = results;
    });

    videofeed.hide();
    noLoop();
}

// p5.js draw function() is called after the setup function
function draw() {
    if (started) {
        image(videofeed, 0, 0, width, height);
        setTimeout('calEyes()', 7000);
    }
}

// toggle button for starting the video feed
function start() {
    select("#startstop").html("stop");
    document.getElementById("startstop").addEventListener("click", stop);
    started = true;
    loop();
}

// toggle button for ending the video feed
function stop() {
    select("#startstop").html("start");
    document.getElementById("startstop").addEventListener("click", start);
    removeblur();
    started = false;
    noLoop();
}
function calculateAngle(A, B, C) {

    var AB = Math.sqrt(Math.pow(A.x - B.x, 2) + Math.pow(A.y - B.y, 2));
    var AC = Math.sqrt(Math.pow(A.x - C.x, 2) + Math.pow(A.y - C.y, 2));
    var BC = Math.sqrt(Math.pow(B.x - C.x, 2) + Math.pow(B.y - C.y, 2));
    var cosA = (Math.pow(AB, 2) + Math.pow(AC, 2) - Math.pow(BC, 2)) / (2 * AB * AC);
    var angleA = Math.round(Math.acos(cosA) * 180 / Math.PI);
    console.log(angleA);
    return angleA;
}

// defining the parameters used for the posenet : the tracking of the eyes
var rightEye,
    leftEye,
    defaultRightEyePosition = [],
    defaultLeftEyePosition = [];

//function to calculate the position of the various keypoints
function calEyes() {
    for (let i = 0; i < poses.length; i++) {
        let pose = poses[i].pose;
        
        leftShoulder = pose.keypoints[5].position;
        rightShoulder = pose.keypoints[6].position;
        lefthip = pose.keypoints[11].position;
        righthip = pose.keypoints[12].position;
        leftKnee = pose.keypoints[13].position;
        rightKnee = pose.keypoints[14].position;
        leftAnkle = pose.keypoints[15].position;
        rightAnkle = pose.keypoints[16].position;
        console.log(leftShoulder);
        console.log(rightShoulder);
        console.log(leftAnkle);
        console.log(rightAnkle);

        var angle = calculateAngle(leftShoulder, lefthip, leftKnee);

        if (angle < 20) {
            stage2 = "down";
        }

        if (angle >= 20 && stage2 == "down") {
            //up
            stage2 = "up";
            upAndDown();
        }

        //if ((Math.abs(leftAnkle.y ) > 400 )) {

        //stage1 ="down";

        //
        //}
        //if ((Math.abs(leftAnkle.y) < 400 && stage1 === "down")) {
        //stage1 = "up";
        //setTimeout() 
        // for(let j=0; j<1; j++){
        //   sleep(3000);
        //   upAndDown();

        // }
        //upAndDown();

        //}
    }
}


function upAndDown() {

    if (count < 100) {
        //sleep(3000);
        count = count + 1;
        let calurie = 0.01 * count

        document.getElementById("count").innerHTML = count;
        document.getElementById("calurie").innerHTML = calurie;
    }


    if (count == 100) {

        alert("You have completed the exercise!");

        count = 101;
        started = false;
        noLoop();

    }


    //if count is greater than 5, then the user is not standing
}

function sleep(ms) {

    var now = new Date();
    var exitTime = now.getTime() + ms;
    while (true) {
        now = new Date();
        if (now.getTime() > exitTime)
            return;

    }
}

//function to blur the background and add audio effect
function blur() {
    document.body.style.filter = "blur(5px)";
    document.body.style.transition = "1s";
    var audio = document.getElementById("audioElement");
    console.log("change");
    audio.play();
}

//function to remove the blur effect
function removeblur() {
    document.body.style.filter = "blur(0px)";
    var audio = document.getElementById("audioElement");

    audio.pause();
}
