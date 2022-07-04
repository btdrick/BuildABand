import { React, useState, useEffect, useRef } from "react";
import { AnimatePresence, motion } from "framer-motion";
import { Link } from "react-router-dom";
import { useClickOutside } from "react-click-outside-hook";
import UserProfile from "../UserProfile";
import SearchIcon from "@mui/icons-material/Search";
import CloseIcon from "@mui/icons-material/Close";
import './searchBar.css';

/* Dimensions for expanded, collapsed search bar */
const containerVariants = {
    expanded: {
      width: "400px",
      height: "20em",
      y: "9em",
    },
    collapsed: {
      height: "40px",
    },
  };

/* Animation transition details */
const containerTransition = { type: "spring", damping: 22, stiffness: 150 };

function SearchBar({ placeholder, data} ) {
    /* Animation attributes */
    const [isExpanded, setExpanded] = useState(false);
    const [parentRef, isClickedOutside] = useClickOutside();
    const inputRef = useRef();
     /* Search Results array */
    const [filteredData, setFilteredData] = useState([]);
    /* Search input */
    const [wordEntered, setWordEntered] = useState("");

    /* Filters results based on first + last name*/
  const handleFilter = (event) => {
    const searchWord = event.target.value;
    setWordEntered(searchWord);
    const newFilter = data.filter((value) => {
        /* Valid search criteria*/
        const validSearch = 
        ((value.Fname + " " + value.Lname)
        .toLowerCase()
        .includes(searchWord.toLowerCase())) ||
        ((value.Instrument)
        .toLowerCase()
        .includes(searchWord.toLowerCase())) ||
        ((value.City)
        .toLowerCase()
        .includes(searchWord.toLowerCase())) || 
        ((value.StateCode)
        .toLowerCase()
        .includes(searchWord.toLowerCase()))

        return validSearch;
    });

    if (searchWord === "") {
      setFilteredData([]);
    } else {
      setFilteredData(newFilter);
    }
  };

    /* Sets expand container to true */
    const expandContainer = () => {
        setExpanded(true);
    };

    /* Set state for container collapse */
    const collapseContainer = () => {
        setExpanded(false);
        setWordEntered("")
        setFilteredData([]);
        if (inputRef.current) inputRef.current.value = "";
    };

    /* Once the page renders, this hook takes place */
    useEffect(() => {
        if (isClickedOutside) collapseContainer();
    }, [isClickedOutside]);

    return(
        <motion.div className="search-bar-container" 
        animate={isExpanded ? "expanded" : "collapsed"}
        variants={containerVariants}
        transition={containerTransition}
        ref={parentRef}>
            {/* Search bar */}
            <div className="search-bar-input-container" 
            variants={containerVariants}
            transition={containerTransition}>
                {/* Search input */}
                <span className="search-bar-icon"><SearchIcon /></span>
                <input className="search-bar-input"
                placeholder={placeholder}
                onFocus={expandContainer}
                ref={inputRef}
                value={wordEntered}
                onChange={handleFilter} />
                {/* Expanded view */}
                <AnimatePresence>
                {isExpanded && (
                    <motion.span 
                    className="close-icon"
                    key="close-icon"
                    initial={{ opacity: 0 }}
                    animate={{ opacity: 1 }}
                    exit={{ opacity: 0 }}
                    onClick={collapseContainer}
                    transition={{ duration: 0.2 }}>
                        <CloseIcon />
                    </motion.span>
                )}
                </AnimatePresence>
            </div>
            {/* Results view */}
            {isExpanded 
            && <span className="line-seperator"></span>}
            {/* Default results */}
            {isExpanded && wordEntered === "" && 
                <div className="data-result">
                {data.slice(0, 20).map((value, index) => {
                    return (
                    <div key={index} className="data-item">
                        <Link className="data-link" to={`/profile/${value.MusicianID}`}>
                            {value.Fname + " " + value.Lname}
                            {" (" + value.Instrument + "): "}
                            {value.City + ", " + value.StateCode}
                        </Link>
                        {UserProfile.setProfileID(value.MusicianID)}
                    </div>
                    );
                })}
                </div>
            }
            {/* Search results */}
            {filteredData.length !== 0 && 
                <div className="data-result">
                {filteredData.slice(0, 20).map((value, index) => {
                    return (
                    <div key={index} className="data-item">
                        <Link className="data-link" to={`/profile/${value.MusicianID}`}>
                            {value.Fname + " " + value.Lname}
                            {" (" + value.Instrument + "): "}
                            {value.City + ", " + value.StateCode}
                        </Link>
                        {UserProfile.setProfileID(value.MusicianID)}
                    </div>
                    );
                })}
                </div>
            }
        </motion.div>
    );
}

export default SearchBar;