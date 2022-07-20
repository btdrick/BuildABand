var ValidateUpdateProfileFormFields = (function() {
    /* Regex patterns */
    const regPatterns = { 
        'Email': /^(("[\w-\s]+")|([\w-]+(?:\.[\w-]+)*)|("[\w-\s]+")([\w-]+(?:\.[\w-]+)*))(@((?:[\w-]+\.)*\w[\w-]{0,66})\.([a-z]{2,6}(?:\.[a-z]{2})?)$)|(@\[?((25[0-5]\.|2[0-4][0-9]\.|1[0-9]{2}\.|[0-9]{1,2}\.))((25[0-5]|2[0-4][0-9]|1[0-9]{2}|[0-9]{1,2})\.){2}(25[0-5]|2[0-4][0-9]|1[0-9]{2}|[0-9]{1,2})\]?$)/i,
        'Phone': /^[0-9]{3}-[0-9]{3}-[0-9]{4}$/i,
        'ZipCode': /(^\d{5}$)|(^\d{5}-\d{4}$)/i
    }

    /* Validates string is not undefined or empty */
    function isEmptyOrWhiteSpaces(value){
      return value === null || value.match(/^ *$/) !== null;
    }

    /* Validates input matches pattern */
    function invalidFormat(input, regex) {
      return input && !regex.test(input);
    }

    const noChangesMade = (o1, o2) =>
        Object.keys(o1).length === Object.keys(o2).length 
            && Object.keys(o1).every(p => o1[p] === o2[p]);

    /* Validate input fields */
    function invalidSubmit(input, musician) {
        let errors = {};

        if(noChangesMade(input, musician)) {
          errors["Input"] = "No changes detected";
          return errors;
        }

        /* Date of Birth validation */
        if (!input.DateOfBirth || input.DateOfBirth === '') {
          errors["DateOfBirth"] = "Please enter your date of birth.";
        }
        /* User must be 16 or older */
        if (input.DateOfBirth !== undefined || '') {
          const inputDate = new Date(input['DateOfBirth']);
          const date16YrsAgo = new Date();
          date16YrsAgo.setFullYear(date16YrsAgo.getFullYear() - 16);
          if (!(inputDate <= date16YrsAgo) ) {
            errors["DateOfBirth"] = "Please enter date of birth equal or above 16 years";
          }
        }

        /* Phone format validation */
        if (isEmptyOrWhiteSpaces(input.Phone) || invalidFormat(input.Phone, regPatterns['Phone'])) {
          errors["Phone"] = "Please enter valid phone number(XXX-XXX-XXXX).";
        }
        
        /* Email format validation */
        if (isEmptyOrWhiteSpaces(input.Email) || invalidFormat(input.Email, regPatterns['Email'])) {
          errors["Email"] = "Please enter a valid email Address.";
        }
  
        /* Address 1 validation */
        if (isEmptyOrWhiteSpaces(input.Address1)) {
          errors["Address1"] = "Please enter your address.";
        }

        /* City validation */
        if (isEmptyOrWhiteSpaces(input.City)) {
          errors["City"] = "Please enter your city.";
        }
    
        /* Zip code validation */
        if (isEmptyOrWhiteSpaces(input.ZipCode) || invalidFormat(input.ZipCode, regPatterns['ZipCode'])) {
          errors["ZipCode"] = "Please enter a valid zip code.";
        }

        if (Object.keys(errors).length) {
          return errors;
        }

        return false;
    }

    return {
        invalidSubmit: invalidSubmit,
    }
})();

export default ValidateUpdateProfileFormFields;