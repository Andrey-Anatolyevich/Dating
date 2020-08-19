import * as React from "react";

interface DatingAdListPagerProps {
    pageCurrent: number;
    itemsTotal: number;
    itemsPerPage: number;
    openPageNumber: (number) => void;
}

export class DatingAdListPager extends React.Component<DatingAdListPagerProps, {}> {
    constructor(props) {
        super(props)
        this.openPageNumber = this.openPageNumber.bind(this);
    }

    private openPageNumber(e: React.MouseEvent<HTMLSpanElement, MouseEvent>) {
        e.preventDefault();
        let pageNumberString = e.currentTarget.getAttribute('data-page-number');
        let pageNumber = Number.parseInt(pageNumberString);
        this.props.openPageNumber(pageNumber);
    }

    public render() {
        let pageMax = Math.ceil(this.props.itemsTotal / this.props.itemsPerPage);
        let pageButtons = [];
        for (let i = 1; i <= pageMax; i++) {
            let isCurrentPage = i == this.props.pageCurrent;
            let className = !isCurrentPage ? "btn" : "btn btn-active";
            let onClick = isCurrentPage ? null : this.openPageNumber;
            let control = <span key={i} className={className} onClick={onClick} data-page-number={i}>{i}</span>
            pageButtons.push(control);
        }
        return (
            <div className="data-block">
                <div id="pages-btns-box" className="ad-list-pager">
                    {pageButtons}
                </div>
            </div >
        );
    }
}
