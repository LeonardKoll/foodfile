import React, { useState } from 'react';
import { Collapse, Container, Navbar, NavbarBrand, NavbarToggler, NavItem, NavLink } from 'reactstrap';
import { Link } from 'react-router-dom';
import './NavMenu.css';


export function NavMenu ({mode})
{
    const [collapsed, setCollapsed] = useState(true);

    return (
      <header>
        <Navbar className="navbar-expand-sm fixed-top navbar-toggleable-sm bg-white border-bottom box-shadow mb-3" light>
          <Container>
            <NavbarBrand tag={Link} to="/">FoodFile</NavbarBrand>
            <NavbarToggler onClick={() => {setCollapsed(!collapsed)}} className="mr-2" />
            <Collapse className="d-sm-inline-flex flex-sm-row-reverse" isOpen={!collapsed} navbar>
              <ul className="navbar-nav flex-grow">
                <NavItem>
                  <NavLink tag={Link} className="text-dark" to="/developers">Developers</NavLink>
                </NavItem>
                {
                  ((mode.Mode=="member") || (mode.Mode=="combined")) &&
                  <NavItem>
                    <NavLink tag={Link} className="text-dark" to="/membership">Membership</NavLink>
                  </NavItem>
                }
                {
                  ((mode.Mode=="regular") || (mode.Mode=="combined")) &&
                  <NavItem>
                    <NavLink tag={Link} className="text-dark" to="/trace">Trace</NavLink>
                  </NavItem>
                }
                {
                  ((mode.Mode=="regular") || (mode.Mode=="combined")) &&
                  <NavItem>
                    <NavLink tag={Link} className="text-dark" to="/capture">Capture</NavLink>
                  </NavItem>
                }
              </ul>
            </Collapse>
          </Container>
        </Navbar>
      </header>
    );
}

/*
export class NavMenu extends Component {
  static displayName = NavMenu.name;

  constructor (props) {
    super(props);

    this.toggleNavbar = this.toggleNavbar.bind(this);
    this.state = {
      collapsed: true
    };
  }

  toggleNavbar () {
    this.setState({
      collapsed: !this.state.collapsed
    });
  }

  render () {
    return (
      <header>
        <Navbar className="navbar-expand-sm fixed-top navbar-toggleable-sm bg-white border-bottom box-shadow mb-3" light>
          <Container>
            <NavbarBrand tag={Link} to="/">FoodFile</NavbarBrand>
            <NavbarToggler onClick={this.toggleNavbar} className="mr-2" />
            <Collapse className="d-sm-inline-flex flex-sm-row-reverse" isOpen={!this.state.collapsed} navbar>
              <ul className="navbar-nav flex-grow">
                <NavItem>
                  <NavLink tag={Link} className="text-dark" to="/developers">Developers</NavLink>
                </NavItem>
                {
                  isMembership &&
                  <NavItem>
                    <NavLink tag={Link} className="text-dark" to="/membership">Membership</NavLink>
                  </NavItem>
                }
                {
                  !isMembership &&
                  <NavItem>
                    <NavLink tag={Link} className="text-dark" to="/trace">Trace</NavLink>
                  </NavItem>
                }
                {
                  !isMembership &&
                  <NavItem>
                    <NavLink tag={Link} className="text-dark" to="/capture">Capture</NavLink>
                  </NavItem>
                }
              </ul>
            </Collapse>
          </Container>
        </Navbar>
      </header>
    );
  }
}
*/