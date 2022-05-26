const previousBtn = document.getElementById('previousBtn');
const nextBtn = document.getElementById('nextBtn');
const bullets = [...document.querySelectorAll('.bullet')];

const MAX_STEPS = 5;
let currentStep = 1;

nextBtn.addEventListener('click', () => {
	LastDiv = "step" + currentStep
	document.getElementById(LastDiv).style.display = "none";

	bullets[currentStep - 1].classList.add('completed');
	currentStep += 1;
	previousBtn.disabled = false;
	if (currentStep === MAX_STEPS) {
		nextBtn.disabled = true;
	}
	ThisDiv = "step" + currentStep
	document.getElementById(ThisDiv).style.display = "block";
});


previousBtn.addEventListener('click', () => {
	LastDiv = "step" + currentStep
	document.getElementById(LastDiv).style.display = "none";

	bullets[currentStep - 2].classList.remove('completed');
	currentStep -= 1;
	nextBtn.disabled = false;
	if (currentStep === 1) {
		previousBtn.disabled = true;
	}
	ThisDiv = "step" + currentStep
	document.getElementById(ThisDiv).style.display = "block";
});
