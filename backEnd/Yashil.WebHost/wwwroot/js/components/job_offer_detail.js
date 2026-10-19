
const sendResumeButton = document.getElementById('sendResumeButton').addEventListener('click', function () {
    sendResume();
});

//این متد عملیات ارسال شناسه شغل برای ثبت کاربر به عنوان کاربران درخواست دهنده شغل کاربرد دارد
async function sendResume() {
    try {
        var response = await fetch('/jobCoUser/AddJobCoUser?corporationJobCoId=' + corporationJobCoId);
        var data = await response.json();

        if (data.returnUrl || (!data.isSuccess && data.message && data.entityId == 0) ||
            (!data.isSuccess && data.message && data.entityId == 1)) {
            handleError(data);
        }
        else if (data.isSuccess && data.message) {
            //حالتی که عملیات با موفقیت کامل انجام شده است
            var sendResumeResultEl = document.getElementById('sendResumeSuccessfulResultBox');
            var sendResumeButton = document.getElementById('sendResumeButton');
            sendResumeButton.classList.add('d-none');
            var sendResumeResultMessageEl = document.getElementById('succssfulResultSpan');
            sendResumeResultEl.classList.remove('d-none');
            sendResumeResultMessageEl.innerHTML = data.message;
        }
    }
    catch (error) {
        bootbox.alert('بروز خطا در برقرار ارتباط با سرور.');
    }
}

//این متد برای مدیریت خطای های رخ داده هنگام ارسال درخواست شغل کاربر کاربرد دارد
function handleError(error) {    
    if (error.returnUrl) {
        //در این حالت یا کاربر لاگین نکرده است یا شناسه شغل ارسالی دارای مقدار غیر معتبر است
        window.location.href = error.returnUrl;
    } else if (!error.isSuccess && error.message && error.entityId == 0) {
        //در این حالت کاربر یا رزومه ندارد یا رزومه خود را تکمیل نکرده است
        //لینک تکمیل یا ایجاد رزومه را نمایش می دهیم
        var sendResumeResultEl = document.getElementById('sendResumeErrorResultBox');
        sendResumeResultEl.classList.remove('d-none');
        sendResumeResultEl.classList.add('send-resume-error-result-box');
        sendResumeResultEl.innerHTML = error.message + '</br> <a href="/resume" class="fs-12" style="color:cornflowerblue;display:flex;justify-content:center">ایجاد یا تکمیل رزومه</a>';
    } else if (!error.isSuccess && error.message && error.entityId == 1) {
        //در این حالت در هنگام درج در دیتابیس خطایی رخ داده است
        var sendResumeResultEl = document.getElementById('sendResumeErrorResultBox');
        sendResumeResultEl.classList.remove('d-none');
        sendResumeResultEl.classList.add('send-resume-error-result-box');
        sendResumeResultEl.innerHTML = error.message

    }
}