import React, { useState } from "react";
import "./search.css";
import SearchIcon from '@mui/icons-material/Search';
import CloseIcon from '@mui/icons-material/Close';

function Search({ placeholder, data }) {
    /* Search Results array */
    const [filteredData, setFilteredData] = useState([]);
    /* Search input */
    const [wordEntered, setWordEntered] = useState("");

    /* Filters results based on first + last name*/
    const handleFilter = (event) => {
        const searchWord = event.target.value;
        setWordEntered(searchWord);
        const newFilter = data.filter( (value) => {
            return (value.Fname + " " + value.Lname).toLowerCase().includes(searchWord.toLowerCase());
        });

        if (searchWord === "") {
            setFilteredData([]);
        } else {
            setFilteredData(newFilter);
        }
    };

    /* Clears search input and results */
    const clearInput = () => {
        setFilteredData([]);
        setWordEntered("");
    };

    return (
    <div className="search">
        {/* Search input */}
        <div className="search-inputs">
            <input
                type="text"
                placeholder={ placeholder }
                value={ wordEntered }
                onChange={ handleFilter }
            />
            {/* Clear button appears if search results exist */}
            <div className="search-icon">
                {filteredData.length === 0 ? (
                <SearchIcon />
                ) : (
                <CloseIcon id="clear-btn" onClick={ clearInput } />
                )}
            </div>
        </div>
        {/* Search results */}
        {filteredData.length !== 0 && (
        <div className="data-result">
            {filteredData.slice(0, 15).map((value, key) => {
            {/** @todo: href must be updated to redirect to appropriate page **/}
            return (
                <a key={value.MusicianID} className="data-item" href={"/#/profile/" + value.MusicianID}>
                <p>{value.Fname + " " + value.Lname} </p>
                </a>
            );
            })}
        </div>
        )}
    </div>
    );
}

export default Search;