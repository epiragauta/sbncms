/// <reference types="Cypress" />
context('Relation Types', () => {

  beforeEach(() => {
    cy.sbnLogin(Cypress.env('username'), Cypress.env('password'));
  });

  it('Create relation type', () => {
    const name = "Test relation type";

    cy.sbnEnsureRelationTypeNameNotExists(name);

    cy.sbnSection('settings');
    cy.get('li .umb-tree-root:contains("Settings")').should("be.visible");

    cy.sbnTreeItem("settings", ["Relation Types"]).rightclick();

    cy.sbnContextMenuAction("action-create").click();

    cy.get('form[name="createRelationTypeForm"]').within(($form) => {
      cy.get('input[name="relationTypeName"]').type(name);

      cy.get('[name="relationType-direction"] input').first().click({force:true});

      cy.get('select[name="relationType-parent"]').select('Document');

      cy.get('select[name="relationType-child"]').select('Media');

      cy.get(".btn-primary").click();
    });

    cy.location().should((loc) => {
      expect(loc.hash).to.include('#/settings/relationTypes/edit/')
    })

    //Clean up
    cy.sbnEnsureRelationTypeNameNotExists(name);
   });

});
