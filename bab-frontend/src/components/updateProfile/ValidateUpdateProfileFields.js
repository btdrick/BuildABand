var ValidateUpdateProfileFormFields = (function() {
    /* Regex patterns */
    const regPatterns = { 
        'Email': /^(("[\w-\s]+")|([\w-]+(?:\.[\w-]+)*)|("[\w-\s]+")([\w-]+(?:\.[\w-]+)*))(@((?:[\w-]+\.)*\w[\w-]{0,66})\.([a-z]{2,6}(?:\.[a-z]{2})?)$)|(@\[?((25[0-5]\.|2[0-4][0-9]\.|1[0-9]{2}\.|[0-9]{1,2}\.))((25[0-5]|2[0-4][0-9]|1[0-9]{2}|[0-9]{1,2})\.){2}(25[0-5]|2[0-4][0-9]|1[0-9]{2}|[0-9]{1,2})\]?$)/i,
        'Phone': /^[0-9]{3}-[0-9]{3}-[0-9]{4}$/i,
        'ZipCode': /(^\d{5}$)|(^\d{5}-\d{4}$)/i
    }

    /* Validates string is not undefined or empty */
    function invalidString(string) {
      return !string || string === "";
    }

    /* Validates input matches pattern */
    function invalidFormat(input, regex) {
      return input && !regex.test(input);
    }

    /* Validate input fields */
    function validSubmit(input) {
        let errors = {};
        let isValid = true;

        /* Date of Birth validation */
        if (!input.DateOfBirth || input.DateOfBirth === '') {
          isValid = false;
          errors["dateOfBirth"] = "Please enter your date of birth.";
        }
        /* User must be 16 or older */
        if (input.DateOfBirth !== undefined || '') {
          const date16YrsAgo = new Date();
          date16YrsAgo.setFullYear(date16YrsAgo.getFullYear() - 16);
          if (!((new Date(input['DateOfBirth'])) <= date16YrsAgo) ) {
            isValid = false;
            errors["dateOfBirth"] = "Please enter date of birth equal or above 16 years";
          }
        }

        /* Phone format validation */
        if (invalidString(input.Phone) || invalidFormat(input.Phone, regPatterns['Phone'])) {
          isValid = false;
          errors["phone"] = "Please enter valid phone number(XXX-XXX-XXXX).";
        }

        /* Email format validation */
        if (invalidString(input.Email) || invalidFormat(input.Email, regPatterns['Email'])) {
          isValid = false;
          errors["email"] = "Please enter a valid email Address.";
        }
  
        /* Address 1 validation */
        if (invalidString(input.Address1)) {
          isValid = false;
          errors["address1"] = "Please enter your address.";
        }

        /* City validation */
        if (invalidString(input.City)) {
          isValid = false;
          errors["city"] = "Please enter your city.";
        }
    
        /* Zip code validation */
        if (invalidString(input.ZipCode) || invalidFormat(input.ZipCode, regPatterns['ZipCode'])) {
          isValid = false;
          errors["zipCode"] = "Please enter a valid zip code.";
        }

        console.log(errors)
        return isValid;
    }

    return {
        validSubmit: validSubmit,
    }
})();

export default ValidateUpdateProfileFormFields;