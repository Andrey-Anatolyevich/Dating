import * as React from "react";
import { BrowserRouter as Router, Redirect, Route, Switch } from "react-router-dom";
import { AdminLocalesList } from '../admin/adminLocalesList';
import { AdminNavBar } from '../admin/adminNavBar';
import { AdminUsersList } from "../admin/adminUsersList";
import { SignIn } from "../auth/signin";
import { SignOut } from "../auth/signOut";
import { DatingList } from '../dating/datingList';
import { NavBar } from "./navBar";
import { AdminPlacesList } from "../admin/adminPlaces";
import { AdminObjectTypes } from "../admin/adminObjectTypes";
import { DatingAdDetailsView } from "../dating/datingAdDetailsView";

//import { PostsList } from "../pages/postsList";
//import { PostItem } from "../pages/postItem";
//import { PostDetails } from "../pages/postDetails";
//import { LoginPage } from "../pages/loginPage";


export class AppShell extends React.Component {
    private initialTitle: string;

    public componentDidMount() {
        this.initialTitle = document.title;
        document.title = 'Dating';
    };

    public componentWillUnmount() {
        document.title = this.initialTitle;
    }

    public render() {
        return (
            <Router>
                <div className="container-page">
                    <header>
                        <AdminNavBar />
                        <NavBar />
                    </header>
                    <div className="block-body block-narrowed">

                        <Switch>
                            <Route exact path="/Admin/Locales/LocalesList" render={props => <AdminLocalesList {...props} />} />
                            <Route exact path="/Admin/ObjectTypes" render={props => <AdminObjectTypes {...props} />} />
                            <Route exact path="/Admin/Places/All" render={props => <AdminPlacesList {...props} />} />
                            <Route exact path="/Admin/Users/UsersList" render={props => <AdminUsersList {...props} />} />
                            <Route exact path="/SignIn" render={props => <SignIn {...props} />} />
                            <Route exact path="/SignOut" render={props => <SignOut {...props} />} />
                            <Route exact path="/Dating" render={props => <DatingList {...props} />} />
                            <Route path="/Dating/ViewAd/:adId" render={props => <DatingAdDetailsView {...props} />} />
                            {/*<Route exact path="/posts" render={props => <PostsList {...props} />} />
                            <Route exact path="/post" render={props => <PostItem {...props} />} />
                            <Route path="/postDetails/:postId" render={props => <PostDetails {...props} />} />*/}
                            <Redirect to="/" />
                        </Switch>

                    </div>


                    <footer className="block-footer">
                        <div className="block-narrowed">
                            <p>&copy; 2018 - 2020 Dating</p>
                        </div>
                    </footer>
                </div>

            </Router>
        );
    }
}
