import * as React from "react";

interface InlinePopupProps {
    onClose(): void
    visible: boolean
    viewSettings: {
        showCloseBtn: boolean
    }
}

interface InlinePopupState {
    counter: number;
}

export class InlinePopup extends React.Component<InlinePopupProps, InlinePopupState> {
    constructor(params) {
        super(params);

        this.state = { counter: 0 };

        this.onOverlayClick = this.onOverlayClick.bind(this);
    }

    private dataTypeOverlay = 'modal-overlay';
    public static defaultProps = {
        visible: false,
        viewSettings: {
            showCloseBtn: true
        }
    };

    private onOverlayClick(e: React.MouseEvent<HTMLDivElement, MouseEvent>): void {
        if ((e.target as HTMLElement).getAttribute('data-type') != this.dataTypeOverlay)
            return;

        this.props.onClose();
    }

    public render() {
        if (!this.props.visible)
            return null;

        return (
            <div data-type={this.dataTypeOverlay} className='modal-overlay' onClick={this.onOverlayClick}>
                <div className='modal-content'>
                    <div className='modal'>
                        {
                            this.props.viewSettings.showCloseBtn != false &&
                            <a className='close' onClick={this.props.onClose}></a>
                        }
                        {
                            this.props.children != null && this.props.children
                        }
                    </div>
                </div>
            </div>
        );
    }
}
