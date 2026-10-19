//   دریافت اطلاعات مربوط به یارانه اشتغال
const employmentSubsidyInformationUrl = '/api/publicApi/GetEmploymentSubsidyInformation';
let employmentSubsidyInformationText = document.querySelectorAll('.employment-subsidy-information');

// Make a GET request
fetch(employmentSubsidyInformationUrl)
    .then(response => {
        if (!response.ok) {
            throw new Error('Network response was not ok');
        }
        return response.json();
    })
    .then(data => {
        // Display data in an HTML element
        if (data.vezaratCeilingPerHekmatCard == data.sataCeilingPerHekmatCard) {
            for (i = 0; i < employmentSubsidyInformationText.length; i++) {
                employmentSubsidyInformationText[i].textContent = Num2persian(data.vezaratCeilingPerHekmatCard / 10) + " تومان یارانه آموزشی ";
            }
        }
        else {
            for (i = 0; i < employmentSubsidyInformationText.length; i++) {
                employmentSubsidyInformationText[i].textContent = Num2persian(data.vezaratCeilingPerHekmatCard / 10) + " تومان یارانه وزارت و  " + Num2persian(data.sataCeilingPerHekmatCard / 10) + " تومان یارانه ساتا ";
            }
        }
    })
    .catch(error => {
        console.error('Error:', error);
    });