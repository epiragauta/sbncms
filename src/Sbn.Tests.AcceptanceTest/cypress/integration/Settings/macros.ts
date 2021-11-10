/// <reference types="Cypress" />
context('Macros', () => {

  beforeEach(() => {
    cy.sbnLogin(Cypress.env('username'), Cypress.env('password'));
  });

  it('Create macro', () => {
    const name = "Test macro";

    cy.sbnEnsureMacroNameNotExists(name);

    cy.sbnSection('settings');
    cy.get('li .umb-tree-root:contains("Settings")').should("be.visible");

    cy.sbnTreeItem("settings", ["Macros"]).rightclick();

    cy.sbnContextMenuAction("action-create").click();

    cy.get('form[name="createMacroForm"]').within(($form) => {
      cy.get('input[name="itemKey"]').type(name);
      cy.get(".btn-primary").click();
    });

    cy.location().should((loc) => {
      expect(loc.hash).to.include('#/settings/macros/edit/')
    });

    //Clean up
    cy.sbnEnsureMacroNameNotExists(name);
   });

});
