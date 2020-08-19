import * as React from "react";
import { NavLink } from "react-router-dom";
import { Http } from "../../../tool/http";
import { DateBox } from "../../../tool/dateBox";

interface AdminUsersListState {
    users: UserInfoJson[];
    failedToLoadData: boolean;
}

export class AdminUsersList extends React.Component<{}, AdminUsersListState> {
    constructor(props) {
        super(props);

        this.state = {
            users: [],
            failedToLoadData: false
        };
    }

    public componentDidMount() {
        new Http()
            .onUrlGet('/Admin/Users/AllUsersJson')
            .onSuccess((responseText) => {
                var response = JSON.parse(responseText) as ApiData<UserInfoJson[]>;
                this.setState({ users: response.data });
            })
            .onError(response => {
                this.setState({ failedToLoadData: true })
            })
            .send();
    }

    public render() {
        return (
            <React.Fragment>
                <h3>Users list</h3>

                <div className="data-block">
                    <table>
                        <thead>
                            <tr>
                                <th>Login</th>
                                <th>Email</th>
                                <th>Registered</th>
                                <th>Last Login</th>
                                <th>Claims</th>
                            </tr>
                        </thead>
                        <tbody>
                            {
                                this.state.users.map((user, index) => {
                                    return <tr key={user.id}>
                                        <th>{user.login}</th>
                                        <th>{user.email}</th>
                                        <th>{DateBox.FromUtcDateString(user.dateCreated).ToLocalFullString()}</th>
                                        <th>{DateBox.FromUtcDateString(user.dateLastLogin).ToLocalFullString()}</th>
                                        <th>{user.claims}</th>
                                    </tr>
                                })
                            }
                        </tbody>
                    </table>
                </div>
            </React.Fragment>
        );
    }
}
