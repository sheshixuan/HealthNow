// Copyright (c) 2019 ml5
//
// This software is released under the MIT License.
// https://opensource.org/licenses/MIT

/* ===
ml5 Example
PoseNet example using p5.js
=== */


let videofeed;
let posenet;
let poses = [];
let started = false;
var audio = document.getElementById("audioElement");
var stage1 = "up";
var count = 0;

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
        setTimeout('calEyes()', 4000);
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

// defining the parameters used for the posenet : the tracking of the eyes
var rightEye,
    leftEye,
    nose,
    defaultRightEyePosition = [],
    defaultLeftEyePosition = [];

//function to calculate the position of the various keypoints
function calEyes() {
    for (let i = 0; i < poses.length; i++) {
        let pose = poses[i].pose;

        rightEye = pose.keypoints[2].position;
        leftEye = pose.keypoints[1].position;
        nose = pose.keypoints[0].position;

        //UP
        if (Math.abs(rightEye.y) < 200) {
            stage1 = "up";

        }
        //DOWN
        if (Math.abs(rightEye.y) > 200 && stage1 == "up") {
            stage1 = "down";
            upAndDown();

        }

    }
}


function upAndDown() {

    if (count < 6) {

        //sleep(3000);
        count = count + 1;
        let calurie = 0.32 * count

        document.getElementById("count").innerHTML = count;
        document.getElementById("calurie").innerHTML = calurie;
    }


    if (count == 6) {

        alert("You have completed the exercise!");

        count = 7;
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
